
/// <reference path="../../ext3.4/ext-base.js" />

/// <reference path="../../ext3.4/ext-all.js" />
/// <reference path="../jquery-1.8.2.min.js" />

Ext.BLANK_IMAGE_URL = '/Scripts/ext3.4/images/default/s.gif';

Ext.namespace("Ext.IIPS");

Ext.IIPS.SysDateDeal = {
    checkEndTime: function () {
        var startTime = $("#startTime").val();
        var start = new Date(startTime.replace("-", "/").replace("-", "/"));
        var endTime = $("#endTime").val();
        var end = new Date(endTime.replace("-", "/").replace("-", "/"));
        if (end < start) {
            return false;
        }
        return true;
    },
    TimePlus: function (startTime, endTime, hourCount) {

        var start = new Date(startTime.replace("-", "/").replace("-", "/"));

        var end = new Date(endTime.replace("-", "/").replace("-", "/"));
        if (((end.getTime() - start.getTime()) / (24 * 60 * 60 * 1000)) > hourCount) {
            return false;
        }
        return true;
    },
    ShowDateLong: function () {
        var date = new Date(); //日期对象

        var year = date.getFullYear(); //读英文就行了
        var month = (date.getMonth() + 1).toString();
        if (month.length == 1) {
            month = "0" + month;
        }
        var day = (date.getDate()).toString();
        if (day.length == 1) {
            day = "0" + day;
        }
        now = now + (date.getMonth() + 1) + "-";//取月的时候取的是当前月-1如果想取当前月+1就可以了
        now = now + date.getDate();
        var hour = date.getHours();
        if (hour.length == 1) {
            hour = "0" + hour;
        }
        var minute = date.getMinutes();
        if (minute.length == 1) {
            minute = "0" + minute;
        }
        var second = date.getSeconds();
        if (second.length == 1) {
            second = "0" + second;
        }
        //now = now + date.getDate() + " ";
        //now = now + date.getHours() + ":";
        //now = now + date.getMinutes() + ":";
        //now = now + date.getSeconds() + "";
        //document.getElementById("nowDiv").innerHTML = now; //div的html是now这个字符串
        //setTimeout("show()", 1000); //设置过1000毫秒就是1秒，调用show方法
        var now = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
        return now;
    },
    ShowTimeLong: function () {
        var date = new Date(); //日期对象

    
        var hour = date.getHours();
        if (hour.length == 1) {
            hour = "0" + hour;
        }
        var minute = date.getMinutes();
        if (minute.length == 1) {
            minute = "0" + minute;
        }
        var second = date.getSeconds();
        if (second.length == 1) {
            second = "0" + second;
        }
        //now = now + date.getDate() + " ";
        //now = now + date.getHours() + ":";
        //now = now + date.getMinutes() + ":";
        //now = now + date.getSeconds() + "";
        //document.getElementById("nowDiv").innerHTML = now; //div的html是now这个字符串
        //setTimeout("show()", 1000); //设置过1000毫秒就是1秒，调用show方法
        var now = hour + ":" + minute + ":" + second;
        return now;
    },
    ShowDatePlus: function (CurDate, dayCount) {
        var now = new Date(CurDate.replace("-", "/").replace("-", "/"));
        if (dayCount >= 1) { now = new Date(now.getTime() - 86400000 * dayCount); }
        var yyyy = now.getFullYear(), mm = (now.getMonth() + 1).toString(), dd = now.getDate().toString();
        if (mm.length == 1) { mm = '0' + mm; } if (dd.length == 1) { dd = '0' + dd; }
        return (yyyy + '-' + mm + '-' + dd);
    },
    ShowDateAdd: function (CurDate, dayCount) {
        var now = new Date(CurDate.replace("-", "/").replace("-", "/"));
        if (dayCount >= 1) { now = new Date(now.getTime() + 86400000 * dayCount); }
        var yyyy = now.getFullYear(), mm = (now.getMonth() + 1).toString(), dd = now.getDate().toString();
        if (mm.length == 1) { mm = '0' + mm; } if (dd.length == 1) { dd = '0' + dd; }
        if (CurDate.indexOf("%20") >= 0) {
            CurDate = CurDate.replace("%20", ' ');
        }
        return (yyyy + '-' + mm + '-' + dd);
    },
    DatePlus: function (start) { //短信收件箱开始时间
        if (start.indexOf("%20") >= 0) {
            start = start.replace("%20", ' ');
        }
        var addTime = moment(start);
        return addTime.format('YYYY-MM-DD HH:mm:ss');
    },
    DateAdd: function (CurDate) { //短信收件箱截止时间
        if (CurDate.indexOf("%20") >= 0) {
            CurDate = CurDate.replace("%20", ' ');
        }
        var addTime = moment(CurDate).add(2, 'days');
        return addTime.format('YYYY-MM-DD HH:mm:ss');
    },
    ShowDate: function () {
        var date = new Date(); //日期对象

        var year = date.getFullYear(); //读英文就行了
        var month = (date.getMonth() + 1).toString();
        if (month.length == 1) {
            month = "0" + month;
        }
        var day = (date.getDate()).toString();
        if (day.length == 1) {
            day = "0" + day;
        }
        now = now + (date.getMonth() + 1) + "-";//取月的时候取的是当前月-1如果想取当前月+1就可以了
        now = now + date.getDate();
        //now = now + date.getDate() + " ";
        //now = now + date.getHours() + ":";
        //now = now + date.getMinutes() + ":";
        //now = now + date.getSeconds() + "";
        //document.getElementById("nowDiv").innerHTML = now; //div的html是now这个字符串
        //setTimeout("show()", 1000); //设置过1000毫秒就是1秒，调用show方法
        var now = year + "-" + month + "-" + day;
        return now;
    }

}
Ext.IIPS.SysBtnInit = {
    LoadSearch: function (container, text, fun, id) {
        $("#" + container).html("<a class='Searchbtn' onclick='" + fun + "()' id='" + id + "'>&nbsp;&nbsp;" + text + "</a>");
    },
    LoadOk: function (container, text, fun, id) {
        $("#" + container).html("<a class='Okbth' onclick='" + fun + "()' id='" + id + "'>&nbsp;&nbsp;" + text + "</a>");
    },
    LoadEdit: function (container, text, fun, id) {
        $("#" + container).html("<a class='Okbth' onclick='" + fun + "()' id='" + id + "'>&nbsp;&nbsp;" + text + "</a>");
    },
    LoadSelect: function (url, ddl, bo, selectedValue, boBlank) {

        var ddlArrays = [];
        //var index = ddl.indexOf('&');
        //if (index > 0)
        ddlArrays = ddl.split('&');
        //else
        //    ddlArrays.push(ddl);
        Ext.Ajax.request({
            url: url,
            timeout: 10000,
            success: function (response, opts) {
                for (var i = 0; i < ddlArrays.length; i++) {
                    if (!bo) {
                        $("#" + ddlArrays[i]).html("");
                        if (boBlank)
                            $("#" + ddlArrays[i]).append("<option value=''>-请选择-</option>");
                        $("#" + ddlArrays[i]).append(response.responseText);

                    }
                    else
                        $("#" + ddlArrays[i]).append(response.responseText);
                    if (selectedValue != null && selectedValue != "")
                        $("#" + ddlArrays[i]).attr("value", selectedValue);
                }
                return true;
            },
            failure: function (response, opts) {
                return false;
            }
        });
    },
    LoadInputSelect: function (displayField, valueField, url, renderCon, id, width, emptyText, fn, fnStoreParam, allowBlank) {
        var store = new Ext.data.Store({
            proxy: new Ext.data.HttpProxy({
                url: url//请求路径    
            }),
            reader: new Ext.data.JsonReader({
                root: 'root',
                totalProperty: 'total'
            }, [
            { name: displayField },
            { name: valueField }
            ]),
            autoLoad: true
        });
        if (fnStoreParam != null) {
            fnStoreParam(store);
        }
        var comboType = new Ext.form.ComboBox({

            store: store,
            id: id,
            displayField: displayField,
            valueField: valueField,
            hiddenName: valueField,//后台接收这个对像的值  
            typeAhead: true,
            mode: 'local',
            triggerAction: 'all',
            emptyText: emptyText,
            selectOnFocus: true,
            applyTo: renderCon,
            matchFieldWidth: false,
            bodyStyle: 'border:1px sold gray;',
            //width: width == null ? 200 : width,
            anchor: 0.99,
            allowBlank: allowBlank,
            listeners: {
                'select': function (combo, record, index) {
                    if (fn != null) {
                        //fn(combo.value);
                        fn();
                    }
                },
                'beforequery': function (e) {

                    var combo = e.combo;
                    if (!e.forceAll) {
                        var input = e.query;
                        // 检索的正则    
                        var regExp = new RegExp(".*" + input + ".*");
                        // 执行检索    
                        combo.store.filterBy(function (record, id) {
                            // 得到每个record的项目名称值    
                            var text = record.get(combo.displayField);
                            return regExp.test(text);
                        });
                        combo.expand();
                        return false;
                    }
                }
            }
        });

    },
    loadStyle: function (url) {
        var link = document.createElement('link');
        link.rel = "stylesheet";
        link.type = "text/css";
        link.href = url;
        var head = document.getElementsByTagName("head")[0];
        head.appendChild(link);
    }
}
Ext.IIPS.SearchFile = function (params)  //关联文件推送查看详情
{
        var arrays = params.split(',');
        var fileTypeId = arrays[0].toString();
        var fileInforId = arrays[1];

        $.ajax({
            type: "GET",
            dataType: "text",
            data: { FileInforId: fileInforId },
            url: "/ComHandlers/InformationFun.ashx?Method=JustifySendOrReceive",
            success: function (obj) {
                if (obj != "") {
                    var arrays = obj.split('|');
                    Ext.IIPS.InformationFileRender.FileDetailClick(arrays[0], fileInforId, null, null, null, fileTypeId, parseInt(arrays[1]), "T");   //InformationFileRender.js 与文件管理列表跳转共用方法
                }
                else {
                    alert("获取文件详情出错!");
                }
            },
            error: function (obj) {
                alert("获取文件详情出错!" + obj);
            }
        });
    
}
Ext.IIPS.LoadFileResHandle = function (listInfors, position, width, height,showClose) {
    var arrays = listInfors.split(',');
    var docWidth = width!=null?width:(document.documentElement.clientWidth - 400);
    var docHeight = height!=null?height:(document.documentElement.clientHeight - 100);
    //var docWidth =document.documentElement.clientWidth - 400;
    //var docHeight = document.documentElement.clientHeight - 50;
    $(arrays).each(function (index, item) {
        $("#div_" + item).show();
        $("#div_" + item).html("正在加载拟办...");
    });
    $.ajax({
        type: "POST",
        dataType: "text",
        data: { FileInforIds: listInfors.replace(/,/g, 'A') },
        url: "/ComHandlers/InformationFun.ashx?Method=GetInforHandlesAndPath",
        success: function (obj) {
            if (obj != "" && obj != null) {

                var jsons = eval("(" + obj + ")");

                for (var i = 0; i < jsons.length; i++) {
                    var FileInforId = jsons[i].InformationID;
                    var FileTypeId = jsons[i].InformationTypeID;
                    var divId = $("#div_" + FileInforId);
                    //var trId = divId[0].parentNode;

                    //$(trId).attr("id", "tr_" + FileInforId);


                    if (jsons[i].HandlerAdvice != "") {
                        $(divId).html("拟办意见:" + jsons[i].HandlerAdvice);
                        $(divId).css("color", "#54668b");
                    }
                    else {
                        $(divId).html("");
                        $(divId).hide("");
                    }
                    var path = jsons[i].HandleResult;
                    $("#tba_" + FileInforId).attr("data-content", path);
                    $("#tba_" + FileInforId).attr("data-title", FileTypeId);

                    Tipped.create("#tba_" + FileInforId, function (element) {
                        var fileId = $(element).attr("id").split('_')[1];
                        if ($(element).data('title') == Ext.IIPS.EnumInforType.Fax) {
                          
                            return {

                                title: "",
                                content: "<iframe id=\"ifrWinRecvRelation\" name=\"ifrWinRecvRelation\"  src=\"/Web/InformationCenter/Fax/FaxOnlineView.aspx?" + fileId + "\" height=\"" + docHeight + "\" width=\"" + docWidth + "\" frameborder=\"0\"></iframe>"
                            };
                        }
                        else {
                            return {

                                title: "",
                                content: "<iframe id=\"ifrWinRecvRelation\" name=\"ifrWinRecvRelation\"  src=\"/Web/InformationCenter/Attachments/AttachOnlineFrame.html?" + $(element).data('content') + "\" height=\"" + docHeight + "\" width=\"" + docWidth + "\" frameborder=\"0\"></iframe>"
                            };
                        }
                    }, {
                        skin: 'light',
                        size: 'small',
                        radius: true,
                        position:position!=null?position: 'left',
               
                        close: showClose!=null?showClose:false,
                        //hideOn: false,
                        onShow: function (content, element) {
                            //alert($(element).attr("class"));

                            $(element).css("background-color", "yellow");
                            //$(element).addClass('highlight');
                        },
                        afterHide: function (content, element) {
                            $(element).css("background-color", "white");
                        }


                    });

                    //$("#tr_" + FileInforId + " a").click(function () {
                    //    alert(123);
                    //});
                }
            }
        }
    });

}


/*****************************************以下查看关联推送文件详情的功能废弃，采用InformationRelationFiles.js的公共方法Ext.IIPS.InformationFileRender.FileDetailClick lxq 20160928*/
Ext.IIPS.SearchFile1 = function (params)  //关联文件推送查看详情
{

    var arrays = params.split(',');
    var fileTypeId = arrays[0].toString();
    var fileInforId = arrays[1];
    var tab = parent.ExtPageFrame.tabCenter;
    var depth = 2;
    if (tab == null)
    {
        tab = parent.parent.ExtPageFrame.tabCenter;
        depth = 3;
    }
   
    var url = "";
    var title = "";
    switch (fileTypeId) {  //收文详情
        case Ext.IIPS.EnumInforType.GAQuickReport.toString():
            if (tab != null) {
                Ext.IIPS.TabAddFiles(depth, "公安信息快报", Ext.IIPS.EnumFileType.ReceiveFile + "|Bulletin/BulletinDetail.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.GAQuickReport, 100, "Web/InformationCenter/InformationCenterFrame.aspx");
            }
            else {
                title = "公安信息快报";
                url = "/Web/InformationCenter/InformationCenterFrame.aspx?" + Ext.IIPS.EnumFileType.ReceiveFile + "|Bulletin/BulletinDetail.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.GAQuickReport;
            }
            break;
        case Ext.IIPS.EnumInforType.Investigation.toString():
            if (tab != null) {
                Ext.IIPS.TabAddFiles(depth, "协查登记表", Ext.IIPS.EnumFileType.ReceiveFile + "|Investigation/InvestigationChangEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.Investigation, 100, "Web/InformationCenter/InformationCenterFrame.aspx");
            } else{
                title = "协查登记表";
                url = "/Web/InformationCenter/InformationCenterFrame.aspx?" + Ext.IIPS.EnumFileType.ReceiveFile + "|Investigation/InvestigationChangEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.Investigation;
            }
            break;
        case Ext.IIPS.EnumInforType.AlarmChange.toString():
            if (tab != null) {//fileType + "|AlarmChange/AlarmChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmChange, 101, "Web/InformationCenter/InformationCenterFrame.aspx"
                Ext.IIPS.TabAddFiles(depth, "警情移交登记表", Ext.IIPS.EnumFileType.ReceiveFile + "|AlarmChange/AlarmChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmChange, 101, "Web/InformationCenter/InformationCenterFrame.aspx");
            } else{
                title = "警情移交登记表";
                url = "/Web/InformationCenter/InformationCenterFrame.aspx?" + Ext.IIPS.EnumFileType.ReceiveFile + "|AlarmChange/AlarmChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmChange;
            }
            break;
        case Ext.IIPS.EnumInforType.AlarmPropertyChange.toString():
            if (tab != null) {//fileType + "|AlarmChange/AlarmChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmChange, 101, "Web/InformationCenter/InformationCenterFrame.aspx"
                Ext.IIPS.TabAddFiles(depth, "警情性质变更申请表", Ext.IIPS.EnumFileType.ReceiveFile + "|AlarmChangeProperty/AlarmPropertyChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmPropertyChange, 101, "Web/InformationCenter/InformationCenterFrame.aspx");
            } else {
                title = "警情性质变更申请表";
                url = "/Web/InformationCenter/InformationCenterFrame.aspx?" + Ext.IIPS.EnumFileType.ReceiveFile + "|AlarmChangeProperty/AlarmPropertyChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmPropertyChange;
            }
            break;
        case Ext.IIPS.EnumInforType.InvestigationRevoke.toString():
            if (tab != null) {//fileType + "|AlarmChange/AlarmChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmChange, 101, "Web/InformationCenter/InformationCenterFrame.aspx"
                Ext.IIPS.TabAddFiles(depth, "撤销协查登记表", Ext.IIPS.EnumFileType.ReceiveFile + "|Investigation/Revoke.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.InvestigationRevoke, 101, "Web/InformationCenter/InformationCenterFrame.aspx");
            } else {
                title = "撤销协查登记表";
                url = "/Web/InformationCenter/InformationCenterFrame.aspx?" + Ext.IIPS.EnumFileType.ReceiveFile + "|Investigation/Revoke.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.InvestigationRevoke;
            }
            break;
        case Ext.IIPS.EnumInforType.InforCard.toString():
            if (tab != null) {//fileType + "|AlarmChange/AlarmChangeEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.AlarmChange, 101, "Web/InformationCenter/InformationCenterFrame.aspx"
                Ext.IIPS.TabAddFiles(depth, "信息卡", Ext.IIPS.EnumFileType.ReceiveFile + "Events/EventDetail.aspx?0|0|" + fileInforId, 106, "Web/InformationCenter/EventsFrame.aspx");
            } else {
                title = "信息卡";
                url = "/Web/InformationCenter/InformationCenterFrame.aspx?" + Ext.IIPS.EnumFileType.ReceiveFile + "Events/EventDetail.aspx?0|0|" + fileInforId + "&" + Ext.IIPS.EnumInforType.InforCard;
            }
            break;
        case Ext.IIPS.EnumInforType.NoTemplate.toString():
            if (tab != null) {
                Ext.IIPS.TabAddFiles(depth, "发文登记", Ext.IIPS.EnumFileType.ReceiveFile + "|NoTemplate/NoTemplateDetail.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.NoTemplate, 109, "Web/InformationCenter/InformationCenterFrame.aspx");
            }
            else {
                title = "发文登记";
                url = "/Web/InformationCenter/InformationCenterFrame.aspx?" + Ext.IIPS.EnumFileType.ReceiveFile + "|NoTemplate/NoTemplateDetail.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.NoTemplate;
            }
            break;
          
    }
    if (tab == null && url != "") {

        parent.parent.Ext.IIPS.FrameFun.RelationWindow(title, url);

    }
}
Ext.IIPS.TabAddFiles = function (depth, name, url, index, frame) {
    if(depth==2)
    {
        parent.ExtPageFrame.addTab(name, url, index, frame);
    }
    else {
        parent.parent.ExtPageFrame.addTab(name, url, index, frame);
    }
}

Ext.IIPS.SendRelationFile = function (fileTypeId, fileInforId) {   //web发文显示存储关联文件
    switch (fileTypeId) {
        case Ext.IIPS.EnumInforType.GAQuickReport.toString():
            parent.parent.ExtPageFrame.addTab("公安信息快报", Ext.IIPS.EnumFileType.SendFile + "|Bulletin/BulletinDetail.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.GAQuickReport, 100, "Web/InformationCenter/InformationCenterFrame.aspx");
            break;
        case Ext.IIPS.EnumInforType.Investigation.toString():
            parent.parent.ExtPageFrame.addTab("协查登记表", Ext.IIPS.EnumFileType.SendFile + "|Investigation/InvestigationChangEdit.aspx?" + fileInforId + "&" + Ext.IIPS.EnumInforType.Investigation, 100, "Web/InformationCenter/InformationCenterFrame.aspx");
            break;
    }

}

