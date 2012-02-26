using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Patcher_Version
    {
        private static Dictionary<int, int> dec_1_2 = new Dictionary<int,int> { 
            {5,0},
            {4,1},
            {7,2},
            {6,3},
            {1,4},
            {0,5},
            {3,6},
            {2,7},
            {13,8},
            {12,9},
            {15,10},
            {14,11},
            {9,12},
            {8,13},
            {11,14},
            {10,15}
        };

        public static bool save_ver(string filename, int one, int two, int three)
        {
            try
            {
                string hex = "AD D7 " + encrypt_1(one) + " D0 " + encrypt_1(two) + " D0 " + encrypt_1(three) + " D0";

                byte[] version_byte = StringToByteArray(System.Text.RegularExpressions.Regex.Replace(hex, @" ", ""));

                System.IO.File.WriteAllBytes(filename, version_byte);
                return true;
            }
            catch { }

            return false;
        }

        public static string load_hex(string filename)
        {
            try
            {
                System.IO.Stream test = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] test_stream = ReadFully(test, 0);

                test.Flush();
                test.Close();

                return HexStr(test_stream);
            }
            catch
            {
                MessageBox.Show("Error reading file - please ensure that they are closed...");
            }

            return "";
        }

        public static string check(string hex_dump)
        {
            string[] words = hex_dump.Split(' ');

            return (String.Format("{0}.{1:D2}.{2:D3}", decrypt_1(words[2]), decrypt_1(words[4]), decrypt_1(words[6])));
        }

        public static int decrypt_1(string version)
        {
            /*
             * Accepts a hex-couple, and converts it to the appropriate version.
             * 
             * */
            string first_char = version.Substring(0, 1);
            string second_char = version.Substring(1, 1);

            int first_int = int.Parse(first_char, System.Globalization.NumberStyles.HexNumber);
            int second_int = int.Parse(second_char, System.Globalization.NumberStyles.HexNumber);

            int return_1;

            if (first_int <= 7)
            {
                return_1 = (Math.Abs((first_int - 7))) * 16;
            }
            else
            {
                return_1 = ((first_int * -1) + 23) * 16;
            }

            int retrun_2 = dec_1_2[second_int];

            return (return_1 + retrun_2);
        }

        public static string encrypt_1(int encode)
        {
            /*
             * Accepts a version numbers, and converts them to the hex variants.
             * 
             * */

            if (encode > 255 || encode < 0)
            {
                return "";
            }

            //Let's split it as before...
            int first_hex_num = encode / 16; //Muahaahha. This will round it off, so if it's 16, we'll get a hex code of 10, which, zimg, is right!
            int second_hex_num = encode - ( first_hex_num * 16 ); //What's remaining...?

            int first_hex_num_e;
            if (first_hex_num < 8) //if its 0-7...
            {
                first_hex_num_e = (Math.Abs((first_hex_num - 7)));
            }
            else
            {
                first_hex_num_e = 23 - first_hex_num;
            }

            int second_hex_num_e = 0;

            foreach (KeyValuePair<int, int> pair in dec_1_2)
            {
                
                if (pair.Value == second_hex_num)
                {
                    second_hex_num_e = pair.Key;
                    break;
                }
            }

            return first_hex_num_e.ToString("X") + second_hex_num_e.ToString("X");
        }

        private static byte[] ReadFully(System.IO.Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static string HexStr(byte[] p)
        {

            char[] c = new char[p.Length * 3];

            byte b;

            for (int y = 0, x = 0; y < p.Length; ++y, ++x)
            {

                b = ((byte)(p[y] >> 4));
                c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = ((byte)(p[y] & 0xF));
                c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = ((byte)(p[y] & 0xF));
                c[++x] = ' ';
            }

            return new string(c);

        }
    }
}
