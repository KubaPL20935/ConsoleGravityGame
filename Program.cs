#if license
MIT License

Copyright (c) 2016-2017 KubaPL20935

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

#endif

#if info

Info:
Made by KubaPL20935/kubapolish. Just for fun. Enjoy!

Changelog:
    0.1.0.0
 ! Initial release.

#endif

using System;
using System.Threading;
#region set values
namespace ConsoleGravityGame
{
    class Program
    {   // set values
        static int[] blocksx = new int[] { };
        static int[] blocksy = new int[] { };
        static char key;
        static string direction = "down";
        static bool canMove;
        static bool canFall;
        static bool jumpMode = false;
        static int jumpTimer = -1;
        static bool pressedR = false;

        static string version = "0.1.0.0";       // game version
        static bool exeMode = true ;          // changes whether game should prompt user to change variables, useful when compiling exe
#endregion                                   //
        #region some values you can change  //
                                           //
        static bool randomBlocks = false; // specify if blocks placement is random or custom                                        // default true
        static int blocksSetId = 1;      // blocks set id, you can view and change them below                                      // default 1
        static bool flyMode = false;    // change to true to allow flying (jumping 2 blocks mid-air)                              // default false
        static int delayms = 150;      // 150 is ok, value changes ms delay between 'frames'                                     // default 150               
        static int rndMin = 75;       // if randomBlocks is true, it changes how much random blocks can it generate, min amount // default 75
        static int rndMax = 175;     // same as above but max                                                                  // default 175
                                    //                                                                                        //
        #endregion                 // controls: WASD/arrows and space, press R to reset (expect bugs and memory leaks)       //
        #region making a box (borders/bounds) and some other things
        static void Main(string[] args)
        {
            Console.WindowHeight++;
            Console.Title = "ConsoleGravityGame v." + version;

            #region exe mode prompts
            if (exeMode)
            {
                Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("\n   Welcome to special EXE Edition. Because this is an EXE and you can't change\n things inside code, you can change variables here.\n(press Enter to set everything to default values)");
                Console.Write("\n\n  Now, tell me:"); Console.Write("\n Blocks are going to be placed randomly or set?\n  (default true) (true/false, true = random, false = set): ");
                while (true)
                {
                    string EXEChoice1 = Console.ReadLine();
                    if (EXEChoice1 == "true" || EXEChoice1 == "") { randomBlocks = true; break; } else if (EXEChoice1 == "false") { randomBlocks = false; break; } else { Console.Write("Error: please type 'true' or 'false'. "); }
                }
                if (!randomBlocks)
                {
                    Console.Write("\n Okay, now tell me, which blocksSetId (level preset) should i use? (1-10)\n(1 - default, some stairs and some flying blocks)\n(2 - parkour) (3 - hookjumps from ee)\n(4-10 - empty, might add something in the future): ");
                    int EXEChoice2; string EXEChoice2s;
                    while (true)
                    {
                        EXEChoice2s = Console.ReadLine();
                        if (EXEChoice2s == "") { blocksSetId = 1; break; }
                        bool EXEBool2 = int.TryParse(EXEChoice2s, out EXEChoice2);
                        if (!(EXEBool2 && EXEChoice2 > 0 && EXEChoice2 < 11)) { Console.Write("Error: please enter a number between 1 and 10. "); } else { blocksSetId = EXEChoice2; break; };
                    }
                }
                else
                {
                    int EXEChoice5; string EXEChoice5s; int EXEChoice6; string EXEChoice6s;
                    Console.Write("\n Okay, now, type minimum amount of random blocks\n that can generate (default 75): ");
                        while (true)
                        {
                            EXEChoice5s = Console.ReadLine();
                            if (EXEChoice5s == "") { rndMin = 75; break; }
                            bool EXEBool5 = int.TryParse(EXEChoice5s, out EXEChoice5);
                            if (!EXEBool5) { Console.Write("Error: please enter a number. "); } else { rndMin = EXEChoice5; break; };
                        }
                        Console.Write("\n Okey, now, type maximum amount of random blocks\n that can generate (default 175): ");
                        while (true)
                        {
                            EXEChoice6s = Console.ReadLine();
                            if (EXEChoice6s == "") { rndMin = 175; break; }
                            bool EXEBool6 = int.TryParse(EXEChoice6s, out EXEChoice6);
                            if (!EXEBool6) { Console.Write("Error: please enter a number. "); } else { rndMax = EXEChoice6; break; };
                        }
                }
                Console.Write("\n Enable hax? (default false) (allow flying; true/false): ");
                while (true)
                {
                    string EXEChoice3 = Console.ReadLine();
                    if (EXEChoice3 == "true") { flyMode = true; break; } else if (EXEChoice3 == "false" || EXEChoice3 == "") { flyMode = false; break; } else { Console.Write("Error: please type 'true' or 'false'. "); }
                }
                int EXEChoice4; string EXEChoice4s;
                Console.Write("\n DelayMs? (default 150) (150 is ok, value changes ms delay\n between 'frames'): ");
                while (true)
                {
                    EXEChoice4s = Console.ReadLine();
                    if (EXEChoice4s == "") { delayms = 100; break; }
                    bool EXEBool4 = int.TryParse(EXEChoice4s, out EXEChoice4);
                    if (!EXEBool4) { Console.Write("Error: please enter a number. "); } else { delayms = EXEChoice4; break; };
                }
                Console.Write("\n\n   Okay, that's all. Have fun!");
                try { char ę = Reader.ReadKey(1500); } catch (TimeoutException) { }
                Console.ResetColor();
                Console.Clear();
            }
            #endregion

            Console.CursorVisible = false;
        start: // label for reset, press R to reset
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(7, 1); Console.Write("Press WASD or arrow keys to move. W or Space to jump. R to reset.");
            Console.SetCursorPosition(25, 23); Console.Write("ConsoleGravityGame v." + version);
            Console.SetCursorPosition(9, 24); Console.Write("Copyright (r) 2016-2017 KubaPL20935. All Rights Reserved.");
            Console.SetCursorPosition(3, 2); Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write(repeatChar('#', 73)); Console.SetCursorPosition(3, 3);
            for (int i = 0; i < 19; i++)
            {
                Console.CursorLeft = 3; Console.Write('#'); Console.CursorLeft = 75; Console.Write('#'); Console.CursorTop++;
            }
            Console.CursorLeft = 3; Console.Write(repeatChar('#', 73));

            //                                     borders: left x=3, right x=75, top y=2, bottom y=22 ?
            Console.SetCursorPosition(23, 19); // Console.ReadKey(true);
        #endregion
            #region blocks x y
            if (!randomBlocks)
            {
                #region custom blocks
                switch (blocksSetId)
                {            // customize blocks location (level editor)
                    case 1: // default set:
                        blocksx = new int[] { 20, 21, 21, 22, 22, 22, 23, 23, 23, 23, 24, 24, 24, 24, 24, 29, 29, 29, 30, 30, 31, 33, 34, 40, 41, 43, 45, 47, 48, 49, 51, 53, 54, 56 };
                        blocksy = new int[] { 21, 21, 20, 19, 20, 21, 21, 20, 19, 18, 17, 18, 19, 20, 21, 21, 20, 19, 20, 21, 21, 16, 15, 20, 19, 18, 17, 17, 16, 15, 14, 13, 11, 8 };
                        break;

                    case 2: // parkour
                        blocksx = new int[] { 74, 74, 73, 7, 4, 4, 4, 5, 4, 7, 7, 7, 8, 9, 11, 13, 15, 17, 20, 23, 26, 29, 33, 37, 42, 46, 50, 54, 58, 63, 67,
                                              72, 74, 70, 69, 67, 65, 63, 61, 59, 57, 55, 53, 50, 47, 47, 47, 47, 47, 47, 47, 47, 47, 46, 45, 46, 45, 44, 43,
                                              42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 43, 42, 42, 41, 40, 40, 39, 38, 38, 37, 36, 36, 35, 34, 34,
                                              33, 32, 32, 31, 41, 39, 37, 35, 33, 43, 31, 45, 46, /*✔*/57, 58, 59, 60, 61, 62/*✔*/, 48, 49, 50, 48, 49, 48,
                                              49, 29, 28, 27, 25, 24, 23, 21, 20, 19, 17, 16, 15, 17, 15, /*g*/ 7, 6, 5, 4, 4, 4, 4, 4, 5, 6, 7, 7, 7, 6, /*g*/
                                              /*g*/ 12, 11, 10, 9, 9, 9, 9, 9, 10, 11, 12, 12, 12, 11 /*g*/ };

                        blocksy = new int[] { 20, 21, 21, 20, 21, 20, 19, 19, 18, 19, 18, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 16, 16, 16,
                                              16, 16, 15, 14, 10, 8, 4, 4, 4, 4, 4, 4, 4, 4, 5, 7, 3, 4, 5, 6, 7, 8, 9, 11, 12, 11, 10, 3, 3, 3, 3, 3, 3, 3,
                                              3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 7, 5, 9, 7, 5, 9, 7, 5, 9, 7, 5, 9, 7, 5, 9, 7, 5, 9, 7, 4, 4, 4, 4, 4, 4, 4, 4,
                                              5, /*✔*/11, 12, 11, 10, 9, 8/*✔*/, 5, 4, 3, 3, 3, 11, 10, 8, 8, 8, 6, 6, 6, 7, 7, 7, 5, 5, 5, 6, 4, /*g*/ 3,
                                              3, 3, 3, 4, 5, 6, 7, 7, 7, 7, 6, 5, 5, /*g*//*g*/ 3, 3, 3, 3, 4, 5, 6, 7, 7, 7, 7, 6, 5, 5 /*g*/ };
                        if (blocksx.Length != blocksy.Length) { Console.Write(blocksx.Length + " " + blocksy.Length); Console.ReadKey(true); Console.CursorLeft -= (blocksx.Length.ToString().Length * 2 + 1); Console.Write(repeatChar(' ', (blocksx.Length.ToString().Length * 2 + 1))); }
                        break;

                    case 3: // hookjumps
                        blocksx = new int[] { 8, 9, 9, 9, 9, 8, 9, 9, 9, 8, 9, 9, 9, 8, 9, 9, 9, 8, 9, 9, 9, 8, 9, 10, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
                                              11, 11, 11, 11, 11, 11, 11, 11, 14, 15, 15, 15, 15, 14, 15, 15, 15, 14, 15, 15, 15, 14, 15, 15, 15, 14, 15, 15,
                                              15, 14, 15, 16, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 20, 21, 21, 21, 21, 20,
                                              21, 21, 21, 20, 21, 21, 21, 20, 21, 21, 21, 20, 21, 21, 21, 20, 21, 22, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
                                              23, 23, 23, 23, 23, 23, 23, 23, 26, 27, 27, 27, 27, 26, 27, 27, 27, 26, 27, 27, 27, 26, 27, 27, 27, 26, 27, 27,
                                              27, 26, 27, 28, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 32, 33, 33, 33, 33, 32,
                                              33, 33, 33, 32, 33, 33, 33, 32, 33, 33, 33, 32, 33, 33, 33, 32, 33, 34, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35,
                                              35, 35, 35, 35, 35, 35, 35, 35, 38, 39, 39, 39, 39, 38, 39, 39, 39, 38, 39, 39, 39, 38, 39, 39, 39, 38, 39, 39,
                                              39, 38, 39, 40, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 44, 45, 45, 45, 45, 44,
                                              45, 45, 45, 44, 45, 45, 45, 44, 45, 45, 45, 44, 45, 45, 45, 44, 45, 46, 47, 47, 47, 47, 47, 47, 47, 47, 47, 47,
                                              47, 47, 47, 47, 47, 47, 47, 47, 50, 51, 51, 51, 51, 50, 51, 51, 51, 50, 51, 51, 51, 50, 51, 51, 51, 50, 51, 51,
                                              51, 50, 51, 52, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 56, 57, 57, 57, 57, 56,
                                              57, 57, 57, 56, 57, 57, 57, 56, 57, 57, 57, 56, 57, 57, 57, 56, 57, 58, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59,
                                              59, 59, 59, 59, 59, 59, 59, 59, 62, 63, 63, 63, 63, 62, 63, 63, 63, 62, 63, 63, 63, 62, 63, 63, 63, 62, 63, 63,
                                              63, 62, 63, 64, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 67, 69, 71, 73, 66, 68,
                                              70, 72, 74, 67, 69, 71, 73, 66, 68, 70, 72, 74, 67, 69, 71, 73, 66, 67, 68, 70, 72, 73, 74, 67, 67, 67, 68, 68,
                                              69, 69, 69, 71, 71, 71, 72, 73, 73 };

                        blocksy = new int[] { 21, 21, 20, 19, 18, 18, 17, 16, 15, 15, 14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7, 8, 9,
                                              10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 21, 20, 19, 18, 18, 17, 16, 15, 15, 14, 13, 12, 12, 11, 10,
                                              9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 21, 20, 19, 18,
                                              18, 17, 16, 15, 15, 14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                                              15, 16, 17, 18, 19, 20, 21, 21, 20, 19, 18, 18, 17, 16, 15, 15, 14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6, 5,
                                              3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 21, 20, 19, 18, 18, 17, 16, 15, 15,
                                              14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                                              20, 21, 21, 20, 19, 18, 18, 17, 16, 15, 15, 14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7,
                                              8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 21, 20, 19, 18, 18, 17, 16, 15, 15, 14, 13, 12, 12, 11,
                                              10, 9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 21, 20, 19,
                                              18, 18, 17, 16, 15, 15, 14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,
                                              14, 15, 16, 17, 18, 19, 20, 21, 21, 20, 19, 18, 18, 17, 16, 15, 15, 14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6,
                                              5, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 21, 20, 19, 18, 18, 17, 16, 15,
                                              15, 14, 13, 12, 12, 11, 10, 9, 9, 8, 7, 6, 6, 5, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
                                              19, 20, 20, 20, 20, 20, 17, 17, 17, 17, 17, 14, 14, 14, 14, 11, 11, 11, 11, 11, 9, 9, 9, 9, 7, 7, 7, 7, 7, 7,
                                              7, 3, 4, 5, 3, 5, 3, 4, 5, 3, 4, 5, 4, 3, 5 };
                                 // this level editor is op 👌
                                // 463 blocks. huh.
                               //\/ Console.Write(blocksx.Length + " " + blocksy.Length); Console.ReadKey(true); Console.CursorLeft-= (blocksx.Length.ToString().Length * 2 + 1); Console.Write(repeatChar(' ',(blocksx.Length.ToString().Length * 2 + 1)));
                        break;//
                             //
                    case 4: // you can add your own coordinates but remember to keep blocksx and blocksy the same length, i.e. if blocksx has 9 entries then blocksy must have 9 entries too
                        blocksx = new int[] { 19, 5, 5, 6, 6, 7, 7, 9, 9 };
                        blocksy = new int[] { 20, 21, 19, 21, 19, 21, 19, 21, 19 };
                        break;

                    case 5: //
                        blocksx = new int[] { 0 };
                        blocksy = new int[] { 0 };
                        break;

                    case 6: //
                        blocksx = new int[] { 0 };
                        blocksy = new int[] { 0 };
                        break;

                    case 7: //
                        blocksx = new int[] { 0 };
                        blocksy = new int[] { 0 };
                        break;

                    case 8: //
                        blocksx = new int[] { 0 };
                        blocksy = new int[] { 0 };
                        break;

                    case 9: //
                        blocksx = new int[] { 0 };
                        blocksy = new int[] { 0 };
                        break;

                    case 10: //
                        blocksx = new int[] { 0 };
                        blocksy = new int[] { 0 };
                        break;

                    default: // when blocksSetId is set to number other than specified above
                        Console.ForegroundColor = ConsoleColor.Red; Console.SetCursorPosition(6,4); Console.Write("Error: wrong blocksSetId. (" + blocksSetId + ") Please use 1-10 (you can change it"); 
                        Console.SetCursorPosition(14,5); Console.Write("and/or add more by modifying source code.)"); Console.CursorVisible = true;
                        Console.ReadKey(true); Environment.Exit(1);
                        break;
                }
                #endregion
            }
            else
            {
                #region random blocks
                Random rnd = new Random();
                int rndYMin = 3;     // top bounds
                int rndYMax = 22;   // bottom bounds
                int rndXMin = 4;   // left bounds
                int rndXMax = 75; // right bounds
                int rndAmount = rnd.Next(rndMin, rndMax + 1);

                blocksy = new int[rndAmount];
                blocksx = new int[rndAmount];

                for (int i = 0; i < blocksy.Length; i++)
                {
                    blocksy[i] = rnd.Next(rndYMin, rndYMax);
                    blocksx[i] = rnd.Next(rndXMin, rndXMax);
                }
                #endregion
            }
            #endregion
            #region generate blocks '&'
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                int i = 0;
                while (true)
                {
                    Console.SetCursorPosition(blocksx[i], blocksy[i]); Console.Write('&'); Console.CursorLeft--;   //\/ Console.ReadKey(true);
                    i++;
                    if (blocksx.Length <= i) { break; }
                }
            }
            #endregion
            #region generate player
            Console.SetCursorPosition(5, 21); // 5, 21: coordinates on where the player will spawn
            if (!flyMode) Console.ForegroundColor = ConsoleColor.Cyan;
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('@'); Console.CursorLeft--;
            Console.ResetColor();
            #endregion
            #region check if block generated inside player
            {
                bool done = false;
                int i = 0;
                while (!done)
                {
                    if (blocksx[i] == Console.CursorLeft && blocksy[i] == Console.CursorTop) { Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write('&'); if (!flyMode) Console.ForegroundColor = ConsoleColor.Cyan; else Console.ForegroundColor = ConsoleColor.Red; Console.Write('@'); Console.CursorLeft--; Console.ResetColor(); }
                    i++;
                    if (blocksx.Length <= i) break;
                }
            }
            #endregion
            #region move logic
#warning this code sux
            while (true)
            {
                canMove = true;
                fallCheck();
                if (canFall && canMove) { move("down"); }
                canMove = true;
                control();
                if (pressedR) { pressedR = false; Console.Clear(); goto start; }
                moveCheck(direction);
                switch (direction)
                {
                    case "left":
                        if (Console.CursorLeft >= 5 && canMove) { move("left"); }
                        break;
                    case "right":
                        if (Console.CursorLeft <= 73 && canMove) { move("right"); }
                        break;

                    case "up":
                        if (jumpMode && flyMode)
                        {
                            jumpTimer = 4;
                        }
                        if (!jumpMode)
                        {
                            jumpMode = true;
                            jumpTimer = 5;
                             if (!randomBlocks && blocksSetId == 3) jumpTimer = 3; // just for 'hookjumps' custom blocks set. you can remove this line if you want
                        }
                        if (jumpMode)
                        {
                            if (Console.CursorTop >= 21)
                            {
                                jumpTimer = 5;
                                if (!randomBlocks && blocksSetId == 3) jumpTimer = 4; // just for 'hookjumps'
                            }
                            int y = 0;
                            foreach (int val in blocksy)
                            {
                                if (val == Console.CursorTop + 1 && blocksx[y] == Console.CursorLeft)
                                {
                                    jumpTimer = 5;
                                    if (!randomBlocks && blocksSetId == 3) jumpTimer = 4; // just for 'hookjumps'
                                    break;
                                }
                                y++;
                            }

                        }
                        break;

                    /// debug up
                    //case "up":
                    //    if (Console.CursorTop >= 4 && canMove) { move("up"); }
                    //    break;
                    /// debug down
                    //case "down":
                    //    if (Console.CursorTop <= 20 && canMove) { move("down"); }
                    //    break;
                }
                canMove = true;
                if (jumpMode)
                {
                    if (jumpTimer >= 2)
                    {
                        moveCheck("up");
                        if (Console.CursorTop >= 4 && canMove) { move("up"); } else { jumpTimer = 0; }
                    }
                    else if (jumpTimer == 0)
                    {
                        moveCheck("down");
                        if (Console.CursorTop <= 20 && canMove) { move("down"); } else { jumpMode = false; jumpTimer = -1; }
                    }

                    // Thread.Sleep(100);
                }

                if (jumpMode && !(jumpTimer == 0)) jumpTimer--;
                if (!jumpMode)
                {
                    canMove = true;
                    moveCheck("down");
                    if (Console.CursorTop <= 20 && canMove)
                    {
                        jumpMode = true; jumpTimer = 0;
                        // Thread.Sleep(100);
                    }
                    // else { canMove = false; if (direction == "up") { moveCheck("up"); if (canMove) move("up"); } }
                }
            }
            #endregion
            #region functions and a class
        }
        static string repeatChar(char Char, int len)
        {
            return new String(Char, len);
        }

        static void fallCheck()
        {
            if (jumpTimer >= 1)
            {
                canFall = false;
                int y = 0;
                foreach (int val in blocksy)
                {
                    if (val == Console.CursorTop + 1 && blocksx[y] == Console.CursorLeft) { canFall = true; }
                    y++;
                }
            }
        }

        static void control()
        {
            try
            {
                key = Reader.ReadKey(delayms);
                //\/Thread.Sleep(delayms);
                
            }
            catch (TimeoutException)
            {
                key = 'S';
            }
            switch (key)
            {
                case 'W':
                case '&':
                case ' ':
                    direction = "up";
                    break;
                case 'A':
                case '%':
                    direction = "left";
                    break;
                case 'S':
                case '(':
                    direction = "down";
                    break;
                case 'D':
                case '\'':
                    direction = "right";
                    break;
                case 'R':
                    pressedR = true;
                    break;
                default:
                    direction = "down"; ;
                    break;
            }

        }

        static void moveCheck(string dir)
        {
            if (dir == "left")
            {
                int x = 0;
                foreach (int val in blocksx)
                {
                    if (val == Console.CursorLeft - 1 && blocksy[x] == Console.CursorTop) { canMove = false; break; }
                    x++;
                }
            }

            if (dir == "right")
            {
                int x = 0;
                foreach (int val in blocksx)
                {
                    if (val == Console.CursorLeft + 1 && blocksy[x] == Console.CursorTop) { canMove = false; break; }
                    x++;
                }
            }

            if (dir == "up")
            {
                int y = 0;
                foreach (int val in blocksy)
                {
                    if (val == Console.CursorTop - 1 && blocksx[y] == Console.CursorLeft) { canMove = false; break; }
                    y++;
                }
            }

            if (dir == "down")
            {
                int y = 0;
                foreach (int val in blocksy)
                {
                    if (val == Console.CursorTop + 1 && blocksx[y] == Console.CursorLeft) { canMove = false; break; }
                    y++;
                }
            }
        }

        static void move(string dir)
        {
            switch (dir)
            {
                case "up":
                    Console.Write(' '); Console.CursorLeft--; Console.CursorTop--; if (!flyMode) Console.ForegroundColor = ConsoleColor.Cyan; else Console.ForegroundColor = ConsoleColor.Red; Console.Write('@'); Console.ResetColor(); Console.CursorLeft--;
                    break;
                case "left":
                    Console.Write(' '); Console.CursorLeft -= 2; if (!flyMode) Console.ForegroundColor = ConsoleColor.Cyan; else Console.ForegroundColor = ConsoleColor.Red; Console.Write('@'); Console.ResetColor(); Console.CursorLeft--;
                    break;
                case "down":
                    Console.Write(' '); Console.CursorLeft--; Console.CursorTop++; if (!flyMode) Console.ForegroundColor = ConsoleColor.Cyan; else Console.ForegroundColor = ConsoleColor.Red; Console.Write('@'); Console.ResetColor(); Console.CursorLeft--;
                    break;
                case "right":
                    if (!flyMode) Console.ForegroundColor = ConsoleColor.Cyan; else Console.ForegroundColor = ConsoleColor.Red; Console.Write(" @"); Console.ResetColor(); Console.CursorLeft--;
                    break;
            }
        }

        class Reader // copy-pasted from google, changed a few things (char instead of string)
        {
            private static Thread inputThread;
            private static AutoResetEvent getInput, gotInput;
            private static char input;

            static Reader()
            {
                getInput = new AutoResetEvent(false);
                gotInput = new AutoResetEvent(false);
                inputThread = new Thread(reader);
                inputThread.IsBackground = true;
                inputThread.Start();
            }

            private static void reader()
            {
                while (true)
                {
                    getInput.WaitOne();
                    input = (char)(Console.ReadKey(true).Key);
                    gotInput.Set();
                }
            }

            public static char ReadKey(int timeOutMillisecs)
            {
                getInput.Set();
                bool success = gotInput.WaitOne(timeOutMillisecs);
                if (success)
                    return input;
                else
                    throw new TimeoutException();
            }

        }
    }
}
        #endregion

/// Copyright 2016-2017, KubaPL20935, All rights reserved.