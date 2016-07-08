using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public class Item
        {
            public string Response { get; set; }    //Response - output string
            public int Mask { get; set; }           //Mask - rule for the item (bits need to be set before current bit could be set)

            public Item(int mask, string resp)
            {
                Mask = mask;
                Response = resp;
            }
        }

        //containes set of rules and responses
        public class tempType
        {
            public Dictionary<int, Item> Lst { set; get; }  // key = command
            public int AllCommands { get; set; } 

            public tempType(string temp)
            {
                if (temp == "HOT")
                {
                    Lst = new Dictionary<int, Item> {
                    {1, new Item(256,"sandals")}, 
                    {2,new Item(256,"sun visor")}, 
                    {3,new Item(1,"")},     //1 - mask for case fail (bit 0 never to be set)
                    {4,new Item(256,"t-shirt")},
                    {5,new Item(1,"")},     //1 - mask for case fail (bit 0 never to be set)
                    {6,new Item(256,"shorts")},
                    {7,new Item(2|4|16|64|256,"leaving house")}, 
                    {8,new Item(0,"Removing PJs")}};
                }
                else
                {
                    Lst = new Dictionary<int, Item> {
                    {1, new Item(256|8,"boots")}, 
                    {2,new Item(256|16,"hat")}, 
                    {3,new Item(256,"socks")}, 
                    {4,new Item(256,"shirt")},
                    {5,new Item(256|16,"jacket")},
                    {6,new Item(256,"pants")}, 
                    {7,new Item(2|4|8|16|32|64|256,"leaving house")}, 
                    {8,new Item(0,"Removing PJs")}};
                }
                AllCommands = Lst[7].Mask | 128; //mask for command 7 - "leave house" and command 7 itself
            }
        }
        static void Main(string[] args)
        {
            string str;
            string strResult;
            List<string> output = new List<string>();
            try
            {
                int mask;
                int key;
                int summ = 0;   //bit position in variable summ = input command number
                tempType p;     
                Console.Write("Input: ");   //read input command
                str = Convert.ToString(Console.ReadLine()).Replace(" ", "");
                // set rule/response object for case selected
                if (str.StartsWith("HOT"))
                {
                    p = new tempType("HOT");
                    str = str.Remove(0, 3);
                }
                else if (str.StartsWith("COLD"))
                {
                    p = new tempType("COLD");
                    str = str.Remove(0, 4);
                }
                else
                {
                    throw new Exception();
                }                
                string[] arr = str.Split(',');
                //process all commands
                foreach (var itm in arr)
                {
                    key = Convert.ToInt32(itm);
                    mask = p.Lst[key].Mask;
                    if (((mask & summ) != mask) || (((1 << key) & summ) > 0)) //apply rule and check for duplicate entry
                    {
                        throw new Exception();
                    };
                    summ |= 1 << key;   //set corresponding bit for current command 
                    output.Add(p.Lst[key].Response);
                }
                //check if all the valid commands have been selected 
                if ((summ & p.AllCommands) != summ) 
                {
                    throw new Exception();
                }              
            }
            catch
            {
                output.Add("fail");
            }
            strResult = string.Join(", ", output);  //build output string
            Console.Write("\nOutput: {0}", strResult);  //write response to console
            Console.ReadLine();
        }
    }
}
