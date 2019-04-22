using SearchService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.Common
{
    public class SearchPage
    {
        private String Style
        {
            get
            {
                return "<STYLE><!--" +
                        "body,td,.p1,.p2,.i{font-family:arial}" +
                        "body{margin:6px 0 0 0;background-color:#fff;color:#000;}" +
                        "table{border:0}" +
                        "TD{FONT-SIZE:9pt;LINE-HEIGHT:18px;}" +
                        ".f14{FONT-SIZE:14px}" +
                        ".f10{font-size:10.5pt}" +
                        ".f16{font-size:16px;font-family:Arial}" +
                        ".c{color:#7777CC;}" +
                        ".p1{LINE-HEIGHT:120%;margin-left:-12pt}" +
                        ".p2{width:100%;LINE-HEIGHT:120%;margin-left:-12pt}" +
                        ".i{font-size:16px}" +
                        ".t{COLOR:#0000cc;TEXT-DECORATION:none}" +
                        "a.t:hover{TEXT-DECORATION:underline}" +
                        ".p{padding-left:18px;font-size:14px;word-spacing:4px;}" +
                        ".f{line-height:120%;font-size:100%;width:32em;padding-left:15px;word-break:break-all;word-wrap:break-word;}" +
                        ".h{margin-left:8px;width:100%}" +
                        ".s{width:8%;padding-left:10px; height:25px;}" +
                        ".m,a.m:link{COLOR:#666666;font-size:100%;}" +
                        "a.m:visited{COLOR:#660066;}" +
                        ".g{color:#008000; font-size:12px;}" +
                        ".r{ word-break:break-all;cursor:hand;width:225px;}" +
                        ".bi {background-color:#D9E1F7;height:20px;margin-bottom:12px}" +
                        ".pl{padding-left:3px;height:8px;padding-right:2px;font-size:14px;}" +
                        ".Tit{height:21px; font-size:14px;}" +
                        ".fB{ font-weight:bold;}" +
                        ".mo,a.mo:link,a.mo:visited{COLOR:#666666;font-size:100%;line-height:10px;}" +
                        ".htb{margin-bottom:5px;}" +
                        "#ft{clear:both;line-height:20px;background:#E6E6E6;text-align:center}" +
                        "#ft,#ft *{color:#77C;font-size:12px;font-family:Arial}" +
                        "#ft span{color:#666}" +
                        "--></STYLE>";
            }
        }

        public String GetStartPage()
        {
            String html = "";
            html += "<html>" + Style + "<body>";
            html += "<input id=\"Keywords\" type=text >";
            html += "&nbsp&nbsp&nbsp";
            html += "<input id=\"Search\" type=submit value=\"搜索\">";
            html += "</body></html>";
            return html;
        }

        public String GetAllResultPage(List<Infors> newsList)
        {
            StringBuilder html = new StringBuilder();
            html.Append(@"<tbody>");

            foreach (Infors news in newsList)
            {
                try
                {
                    GetAllLine(news, html);
                }
                catch
                {
                }
            }

            html.Append("</tbody>");
            return html.ToString();
        }

        private void GetAllLine(Infors news, StringBuilder html)
        {
            //string strFormatHtml = (@"<tr style='height:26px' ><td style='vertical-align:top'>"+
            //                            "<img src='imgs/book.png'/>&nbsp;<a style='font-size:16px' onclick ='FunTransDetails({5})>{1}</a>" +
            //                           "<i style='font-size:8px'>&nbsp;日期：{2}&nbsp总计：{3}字节</i><br/>"+
            //                           "<a style='font-size:8px'  onclick ='FunTransDetails({5}) target='_blank'>{4}</a> &nbsp;" +
            //                            "<div id='div_{0}' /></td></tr>");
            //if (news.Urls.IndexOf(";") > 0)
            //{

            //}
            string[] urls = news.Urls.Split(';');
            string strFormatHtml = "";
            if (urls.Length >= 2)
            {
                strFormatHtml = ("<tr style='height:26px' ><td style='vertical-align:top;margin-bottom:5px;'>" +
                                            "<a style='font-size:16px;margin-bottom:5px' href='{6}' target='_bank'><img src='Images/book.png' /></a>&nbsp;&nbsp;<a a style='font-size:16px;margin-bottom:5px' href='{7}' target='_bank'><img src='Images/book.png' /></a>&nbsp;&nbsp;<a style='font-size:16px;margin-bottom:5px' href='{6}' target='_bank'>{1}</a>" +
                                            "<i style='font-size:10px;margin-bottom:5px;'>&nbsp;日期：{2}&nbsp总计：{3}字节</i><br/><a style='font-size:14px;color:blue;' id='tba_{0}'>【详情】</a>{4}" +
                                            "<br/></td></tr>");//<div id='div_{0}' style='display:none' />
                strFormatHtml = String.Format(strFormatHtml, news.InformationID, news.Title, news.Time.ToString("d"), news.Content.Length, news.Summary, news.TypeID + "," + news.InformationID, urls[0], urls[1]);
            }
            else
            {
                strFormatHtml = ("<tr style='height:26px' ><td style='vertical-align:top;margin-bottom:5px;'>" +
                                            "<a a style='font-size:16px;margin-bottom:5px' href='{6}' target='_bank'><img src='Images/book.png' /></a>&nbsp;&nbsp;<a style='font-size:16px;margin-bottom:5px' href='{6}' target='_bank'>{1}</a>" +
                                            "<i style='font-size:10px;margin-bottom:5px;'>&nbsp;日期：{2}&nbsp总计：{3}字节</i><br/><a style='font-size:14px;color:blue;' id='tba_{0}'>【详情】</a>{4}" +
                                            "<br/></td></tr>");//<div id='div_{0}' style='display:none' />
                strFormatHtml = String.Format(strFormatHtml, news.InformationID, news.Title, news.Time.ToString("d"), news.Content.Length, news.Summary, news.TypeID + "," + news.InformationID, urls[0]);
            }
            
            html.Append(strFormatHtml);
        }

        public String GetSensResultPage(List<Infors> newsList)
        {
            StringBuilder html = new StringBuilder();
            html.Append(@"<tbody>");

            foreach (Infors news in newsList)
            {
                try
                {
                    GetSensLine(news, html);
                }
                catch
                {
                }
            }

            html.Append("</tbody>");
            return html.ToString();
        }

        private void GetSensLine(Infors news, StringBuilder html)
        {
            string strFormatHtml = ("<tr style='height:20px' ><td style='vertical-align:top'>" +
                                        "<a style='font-size:12px' onclick=\"Ext.IIPS.SearchFile('{0}');\"  href='javascript:void(0)'>{1}</a>" +
                                        "<i style='font-size:8px'>&nbsp;日期：{2}</i><br/>{3}<br/>" +
                                        "<a class=\"link\" href='javascript:void(0)' onclick='Ext.IIPS.SearchFile('{0}');' target='_blank'>{4}</a></td></tr><tr style='height:15px'><td></td></tr>");

            strFormatHtml = String.Format(strFormatHtml, news.InformationID, news.Title, news.Time.ToString("yyyy-MM-dd"), news.Summary, "");
            html.Append(strFormatHtml);

        }
    }
}
