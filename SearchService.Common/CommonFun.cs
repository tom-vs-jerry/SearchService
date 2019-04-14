using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Common
{
    public class CommonFun
    {
        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="number">eg: 22</param>
        /// <returns></returns>
        public static string NumberToChinese(int number)
        {
            string res = string.Empty;
            string str = number.ToString();
            res = NumberToChinese(str);
            return res;
        }

        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="number">eg: "22"</param>
        /// <returns></returns>
        public static string NumberToChinese(string number)
        {
            string res = string.Empty;
            string str = number;
            string schar = str.Substring(0, 1);
            switch (schar)
            {
                case "1":
                    res = "壹";
                    break;
                case "2":
                    res = "贰";
                    break;
                case "3":
                    res = "叁";
                    break;
                case "4":
                    res = "肆";
                    break;
                case "5":
                    res = "伍";
                    break;
                case "6":
                    res = "陆";
                    break;
                case "7":
                    res = "柒";
                    break;
                case "8":
                    res = "捌";
                    break;
                case "9":
                    res = "玖";
                    break;
                default:
                    res = "零";
                    break;
            }
            if (str.Length > 1)
            {
                //    switch (str.Length)
                //    {
                //        case 2:
                //        case 6:
                //            res += "十";
                //            break;
                //        case 3:
                //        case 7:
                //            res += "百";
                //            break;
                //        case 4:
                //            res += "千";
                //            break;
                //        case 5:
                //            res += "万";
                //            break;
                //        default:
                //            res += "";
                //            break;
                //    }
                res += NumberToChinese(str.Substring(1, str.Length - 1));
            }
            return res;
        }
    }
}
