<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SearchService.WebServer.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>视频搜索</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="Style/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="Style/PageApply.css" />
    <link rel="stylesheet" type="text/css" href="Style/Paging.css" />
    <link href="Style/tipped.css" rel="stylesheet" />
    <style type="text/css">
        body {
            font-size: 12px;
        }

        table.tableView {
            font-size: 12px;
            text-align: left;
            border: 0px solid;
            border-collapse: collapse;
        }

            table.tableView tr td {
                font-size: 14px;
                text-align: left;
                border: 0px solid;
                padding: 10px;
            }

            table.tableView span {
                font-size: 14px;
            }

        .ui-autocomplete-loading {
            background: white url("Images/ui-anim_basic.gif") right center no-repeat;
        }

        .container {
            height: 70px;
            width: 100%;
            margin: 0px auto 0 auto;
        }

        .search {
            width: 500px;
            height: 40px;
            border-radius: 18px;
            outline: none;
            border: 1px solid #ccc;
            padding-left: 20px;
            position: relative;
        }

        .img {
            height: 35px;
            width: 35px;
            position: relative;
            /*background: url("Images/s.jpg");*/
            top: 13px;
            left: -50px;
            border: none;
            outline: none;
            cursor: pointer;
        }
    </style>
    <script type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>
    <script type='text/javascript' src='Scripts/jquery-ui.js'></script>
    <script type='text/javascript' src='Scripts/socket.io.min.js'></script>
    <script type="text/javascript" src="Scripts/jquery.paging.js"></script>
</head>
<body style="text-align: center">
    <div style="height: 20px; width: 100%"></div>

    <div style="margin: 0 auto; text-align: center; width: 100%">
        <table style="width: 100%">
            <tr style="width: 100%">
                <td style="text-align: center; width: 100%">
                    <div class="container">
                        <input class="search" placeholder="搜索" id="iptInputWord" />
                        <input type="hidden" id="iptWord" />
                        <img src="Images/search.jpg" id="submit" class="img" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div style="text-align: right;">

        <span id="spnTotal" style="color: red; font-size: 12px"></span>
        <hr style="height: 5px; border: none; border-top: 1px outset AliceBlue;" />
    </div>
    <div style="margin: 0 auto; text-align: center; background-color: #fefefe; border-top: 0px solid #e8e8e8; border-bottom: 1px solid #e8e8e8; width: 55%">
        <table style="width: 100%; align-content: center; align-items: center; align-self: center;" class="tableView">
            <tbody style="text-align: center">
                <tr style="text-align: center">
                    <td style="width: 100%; vertical-align: top; empty-cells: hide;">
                        <table id="result" style="width: 100%; margin-right: auto; margin-top: auto; margin-bottom: auto; height: 730px">
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>

            </tbody>
        </table>

    </div>
    <div style="text-align: center; padding-top: 5px;">
        <div id="Pagination" class="meneame"></div>
    </div>
    <div>
        <table style="width: 100%; height: 43px; border-collapse: collapse; border: 0px;">
            <tr>
                <td ></td>
                <td style="text-align: center; color: black; font-weight: 700;" >
                    <!--<img src="imgs/logo.png" style="height:23px; width:23px;" />-->
                </td>
                <td ></td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        var flagOpen = false;
        ////获取index页面传值
        //var thisURL = decodeURI(document.URL);

        //var getval = thisURL.split('?')[1];//key=南山,noneword,starttime,endtime
        var allWord = "";//传值key  南山,山南海北,2016-01-17 14:23:06,2016-05-17 14:23:11
        var showval = "";//keyWord
        var NoneWord = "";//不出现的字

        //默认当前时间为结束时间
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + date.getSeconds();

        //默认开始时间为当前时间一年以前
        //var date1 = new Date(date.getTime() - 365 * 24 * 3600 * 1000 * 10);
        //var month = date1.getMonth() + 1;
        //var strDate = date1.getDate();
        //if (month >= 1 && month <= 9) {
        //    month = "0" + month;
        //}
        //if (strDate >= 0 && strDate <= 9) {
        //    strDate = "0" + strDate;
        //}
        //var sevenday = date1.getFullYear() + seperator1 + month + seperator1 + strDate
        //    + " " + date1.getHours() + seperator2 + date1.getMinutes()
        //    + seperator2 + date1.getSeconds();


        var startTime = "2000-01-01";//开始时间        
        var endTime = currentdate;//结束时间
        var DocNum = "2020";//allWord.split(';')[4];
        var TypeID = 0;//allWord.split(';')[5];
        var SUnitID = ";";// allWord.split(';')[6];
        var RUnitID = ";";// allWord.split(';')[7];
        var Range = 0;//0为title和context；1为context；2为title

        //var inputKey = "";
        var oldinputKey = "";
        var oldnonword = "";
        var oldStartTime = "";
        var oldEndTime = "";
        var oldTypeID = "";
        var oldDocNum = "";
        var oldRecNum = "";


        var oldPageNo = 1;
        var pageSize = 6;
        var Paging;

        var socket = new WebSocket('ws://127.0.0.1:2020');

        //收到server的连接确认
        socket.onopen = function () {
            //连接成功，即可以开始初始化翻页控件
            Paging = $('#Pagination').paging(100, {
                format: '[<nnncnnn!>]',  //格式
                perpage: pageSize,//每页数量
                lapping: 0, //起始页
                page: 1,    //当前页  ["76","88"]
                onSelect: function (page) {
                    var inputKey = $('#iptInputWord').val();
                    if (oldinputKey != inputKey || oldnonword != NoneWord || oldStartTime != startTime || oldEndTime != endTime || oldTypeID != TypeID || oldDocNum != DocNum) {

                        oldinputKey = inputKey;
                        var pageNo = page;
                        var input = 'RI {"InPutWord":"' + inputKey + '","StartTime":"' + startTime + '","EndTime":"' + endTime + '","PageNo":' + pageNo + ',"PageSize":' + pageSize + '}';
                        //var input = '{"Q":"C","P":["' + inputKey + '","' + noneWord + '","' + startTime + '","' + endTime + '","' + InfoID + '","' + TypeID + '","' + RUnitID + '","' + SUnitID + '","' + pageNo + '","' + pageSize + '"]}';
                        debugger
                        socket.send(input);

                        oldinputKey = inputKey;
                        oldnonword = NoneWord;
                        oldStartTime = startTime;
                        oldEndTime = endTime;
                        oldTypeID = TypeID;
                        oldDocNum = DocNum;
                    } else if (oldPageNo != page) {
                        debugger
                        oldPageNo = page;
                        var pageNo = page;
                        var input = 'RI {"InPutWord":"' + inputKey + '","StartTime":"' + startTime + '","EndTime":"' + endTime + '","PageNo":' + pageNo + ',"PageSize":' + pageSize + '}';
                        //var input = '{"Q":"C","P":["' + inputKey + '","' + noneWord + '","' + startTime + '","' + endTime + '","' + InfoID + '","' + TypeID + '","' + RUnitID + '","' + SUnitID + '","' + pageNo + '","' + pageSize + '"]}';
                        debugger
                        socket.send(input);
                    }
                    //return true;
                },
                onFormat: function (type) {
                    switch (type) {
                        case 'block':
                            if (!this.active) {
                                return '<span class="disabled">' + this.value + '</span>';
                            } else if (this.value != this.page) {
                                return '<em><a href="#' + this.value + '">' + this.value + '</a></em>';
                            }
                            return '<span class="current">' + this.value + '</span>';

                        case 'next':

                            if (this.active) {
                                return '<a href="#' + this.value + '" class="next">下一页</a>';
                            }
                            return '<span class="disabled">下一页</span>';

                        case 'prev':

                            if (this.active) {
                                return '<a href="#' + this.value + '" class="prev">上一页</a>';
                            }
                            return '<span class="disabled">上一页</span>';

                        case 'first':

                            if (this.active) {
                                return '<a href="#' + this.value + '" class="first">第一页</a>';
                            }
                            return '<span class="disabled">第一页</span>';

                        case 'last':

                            if (this.active) {
                                return '<a href="#' + this.value + '" class="last">最后一页</a>';
                            }
                            return '<span class="disabled">最后一页</span>';

                        case "leap":

                            if (this.active)
                                return "...";
                            return "";

                        case 'fill':

                            if (this.active)
                                return "...";
                            return "";
                    }
                }
            });

        };

        socket.onmessage = function (json) {
            debugger //服务端发送回来的数据处理
            var a = JSON.parse(json.data);
            $('#result').html(a.R[0]);
            //if (oldRecNum != a.R[4]) {
            Paging.setNumber(parseInt(a.R[4]));
            Paging.setPage(); // reload pagination}
            oldRecNum = a.R[4];
            //}
            $("#spnTotal").html("  总记录数：" + parseInt(a.R[4]));
        };

        //联想功能
        $('#iptInputWord').autocomplete({
            source: function (request, response) {
                //获取输入数据
                var input = request.term.trim();
                if (input != '') {
                    if (oldinputKey != input) {
                        //格式化请求{"Q":"K","P":["request","term"]}
                        //var input = '{"Q":"K","P":["' + request.term + '"]}';
                        var input = 'SA {"Input":"' + request.term + '","Count":20}';
                        debugger
                        socket.send(input);
                    }
                    oldinputKey = input;
                }
                socket.onmessage = function (json) {
                    debugger
                    var a = JSON.parse(json.data);
                    if (a.Q == "SA") {
                        response(a.R);
                        //$("#spnTotal").html("  总记录数：" + parseInt(a.R[3]));
                    }
                }
            },
            select: function (event, ui) {
                //
                $('#iptWord').val(ui.item.value);
            }

        });

        //高级搜索的展开与收起
        function showorhide(id, name) {
            //document.getElementById('divblank').style.display = 'block';
            flagOpen = true;
            var o = document.getElementById(id);
            o.style.display = o.style.display == 'block' ? 'none' : 'block';
            if (o.style.display == 'block') {
                $("#divsearch").css("height", "130px");
                $("#advsearch").css("border-bottom", " 2px solid #f2f2e8");
                //debugger
                //$('#iframeid').prop('contentWindow').document.getElementById('txtfileTypeID').value = SUnitID + "," + RUnitID;
                var ifrmaDoc = $('#iframeid').prop('contentWindow').document;
                if (ifrmaDoc != null) {
                    var tecc = ifrmaDoc.getElementById('txtNoneWord');
                    ifrmaDoc.getElementById('txtNoneWord').value = NoneWord;
                    ifrmaDoc.getElementById('txtWorkStartTime').value = startTime;
                    ifrmaDoc.getElementById('txtWorkStartEnd').value = endTime;
                    ifrmaDoc.getElementById('txtdocnum').value = DocNum;//文件编号
                    ifrmaDoc.getElementById('txtfileTypeID').value = TypeID;

                    if (ifrmaDoc.getElementById('txtfileTypeID').value != "") {
                        var selectTypeID = ifrmaDoc.getElementById('txtfileTypeID').value;
                        if (selectTypeID.substring(selectTypeID.length - 1, selectTypeID.length) == ',') {
                            selectTypeID = selectTypeID.substring(0, selectTypeID.length - 1);
                        }
                        selectTypeID = selectTypeID.split(',');

                        $('#iframeid').prop('contentWindow').taskstatuscombo.setValue(selectTypeID);//设置
                    } else {
                        $('#iframeid').prop('contentWindow').taskstatuscombo.selectAll();
                    }
                }

            }
            else if (o.style.display == 'none') {
                $("#divsearch").css("height", "0px");
            }
            $("#" + name + "").val($("#" + name + "").val() == '收起△' ? '高级▽' : '收起△');

        }


        //提交按钮单击事件
        $('#submit').click(function () {


            var keyWord = $('#iptInputWord').val();

            if (flagOpen) {
                var ifrmaDoc = $('#iframeid').prop('contentWindow').document;
                if (ifrmaDoc != null) {
                    NoneWord = ifrmaDoc.getElementById('txtNoneWord').value == null ? NoneWord : ifrmaDoc.getElementById('txtNoneWord').value;
                    startTime = ifrmaDoc.getElementById('txtWorkStartTime').value == null ? startTime : ifrmaDoc.getElementById('txtWorkStartTime').value;
                    endTime = ifrmaDoc.getElementById('txtWorkStartEnd').value == null ? endTime : ifrmaDoc.getElementById('txtWorkStartEnd').value;
                    DocNum = ifrmaDoc.getElementById('txtdocnum').value == null ? DocNum : ifrmaDoc.getElementById('txtdocnum').value;//文件编号
                    TypeID = ifrmaDoc.getElementById('txtfileTypeID').value == null ? TypeID : ifrmaDoc.getElementById('txtfileTypeID').value;
                }
            }

            if (oldinputKey != inputKey || oldnonword != NoneWord || oldStartTime != startTime || oldEndTime != endTime || oldTypeID != TypeID || oldDocNum != DocNum) {
                oldinputKey = inputKey;
                oldnonword = NoneWord;
                oldStartTime = startTime;
                oldEndTime = endTime;
                oldTypeID = TypeID;
                oldDocNum = DocNum;

                var pageNo = "1";
                var inputKey = $('#iptInputWord').val();
                //拼接发送字符串"{"Q":"C","P":["1","2","3"]}",参数依次为：输入字符，当前页码，每页数量
                //var input = '{"Q":"C","P":["' + inputKey + '","' + noneWord + '","' + startTime + '","' + endTime + '","' + InfoID + '","' + TypeID + '","' + RUnitID + '","' + SUnitID + '","' + pageNo + '","' + pageSize + '"]}';
                var input = 'RI {"InPutWord":"' + inputKey + '","StartTime":"' + startTime + '","EndTime":"' + endTime + '","PageNo":' + pageNo + ',"PageSize":' + pageSize + '}';//'RI {"DocNum":"' + DocNum + '","InPutWord":"' + inputKey + '","Range":' + Range + ',"NoneWord":"' + NoneWord + '","StartTime":"' + startTime + '","EndTime":"' + endTime + '","PageNo":' + pageNo + ',"PageSize":' + pageSize + '}';
                debugger
                socket.send(input);
            }

            //socket接收数据事件
            socket.onmessage = function (json) {
                debugger
                //转化成对象
                var a = JSON.parse(json.data);
                if (a.Q == "RI") {
                    $('#result').html(a.R[0]);
                    var inforID = a.R[1];
                    //设置总记录数、当前页码数
                    Paging.setNumber(parseInt(a.R[4]));
                    Paging.setPage(parseInt(a.R[3]));
                    Paging.setPage(); // reload pagination}
                    $("#spnTotal").html("  总记录数：" + parseInt(a.R[4]));
                    oldPageNo = parseInt(a.R[3]);
                }

            }
        });

    </script>
</body>
</html>
