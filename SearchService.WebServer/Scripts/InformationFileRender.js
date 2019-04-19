Ext.IIPS.InformationFileRender = {};
/**********************************************************************************************标题、编号、概要等列表项重新渲染为可点击列*/
Ext.IIPS.InformationFileRender.FileDetailRender = function (record, text, rowIndex, columnIndex) {
    var linkDetail = "<a class='link' title='" + text + "' id='linkDetail_" + rowIndex + "_" + columnIndex + "' onclick=\"Ext.IIPS.InformationFileRender.FileDetailClick('" + record.data.FILES_TYPE +
                          "','" + record.data.FILES_INFOR_ID + "','" + record.data.INFOR_SEND_ID + "','"
                          + record.data.INFOR_RECEIVE_ID + "','"
                             + record.data.BUSSINESS_ID + "','"
                          + record.data.FILES_INFOR_TYPE_ID + "','"
                          + record.data.SOURCE_TYPE + "')\">"
                          + text + "</a>";
    //$("#linkDetail_" + rowIndex + "_" + columnIndex).manhua_hoverTips({ value: 10 });//使用默认参数，所以不用调用最简洁
    return linkDetail;
}

/**********************************************************************************************点击查看文件详情事件*/
Ext.IIPS.InformationFileRender.FileDetailClick = function (fileType, fileInforId, fileSendId, fileReceiveId, fileBussinessId, inforTypeId, sourceType,ViewDes) {

    switch (fileType) {
        case Ext.IIPS.EnumFileType.NewFile.toString():
            Ext.IIPS.InformationFileRender.FileDetailNewFile(fileType, fileInforId, fileSendId, fileReceiveId, fileBussinessId, inforTypeId);
            break;
        case Ext.IIPS.EnumFileType.SendFile.toString():
            Ext.IIPS.InformationFileRender.FileDetailSendFile(fileType, fileInforId, fileSendId, fileReceiveId, fileBussinessId, inforTypeId, ViewDes);
            break;
        case Ext.IIPS.EnumFileType.ReceiveFile.toString():
            Ext.IIPS.InformationFileRender.FileDetailReceiveFile(fileType, fileInforId, fileSendId, fileReceiveId, fileBussinessId, inforTypeId, sourceType, ViewDes);
            break;
        case Ext.IIPS.EnumFileType.InforCardFile.toString():
            parent.ExtPageFrame.addTab("信息卡", "Events/EventDetail.aspx?0|0|" + fileInforId, 106, "Web/InformationCenter/EventsFrame.aspx","Web/InformationCenter/EventsFrame.aspx",true);
            break;
    }
}
/*************************************************Ext.IIPS.InformationFileRender.FileDetailNewFile无作用***/
Ext.IIPS.InformationFileRender.FileDetailNewFile = function (fileType, fileInforId, fileSendId, fileReceiveId, fileBussinessId, inforTypeId) {
    var url = "";
    var title = "";
    var frame = "/Web/InformationCenter/InformationCenterFrame.aspx";
    var index = 0;

    switch (inforTypeId) {
        case Ext.IIPS.EnumInforType.GAQuickReport.toString():
            title = "(FW)公安信息快报";
            url = "Bulletin/BulletinEdit.aspx";
            index = 100;
            break;
        case Ext.IIPS.EnumInforType.AlarmChange.toString():
            title = "(FW)警情移交登记表";
            url = "AlarmChange/AlarmChangeEdit.aspx";
            index = 101;
            break;
        case Ext.IIPS.EnumInforType.AlarmPropertyChange.toString():
            title = "(FW)警情性质变更申请表";
            url = "AlarmChangeProperty/AlarmPropertyChangeEdit.aspx";
            index = 102;
            break;
        case Ext.IIPS.EnumInforType.Investigation.toString():
            title = "(FW)协查登记表";
            url = "Investigation/InvestigationChangEdit.aspx";
            index = 103;
            break;
        case Ext.IIPS.EnumInforType.InvestigationRevoke.toString():
            title = "(FW)撤销协查登记表";
            url = "Investigation/Revoke.aspx";
            index = 104;
            break;
        case Ext.IIPS.EnumInforType.InforCard.toString():
            title = "(FW)信息卡";
            frame = "/Web/InformationCenter/EventsFrame.aspx";
            url = "Events/EventDetail.aspx";
            index = 105;
            break;

        case Ext.IIPS.EnumInforType.NoTemplate.toString():
            title = "(FW)发文登记";
            url = "NoTemplate/NoTemplateEdit.aspx";
            index = 106;
            break;
    }
    url = fileType + "|" + url + "?" + fileInforId + "&" + inforTypeId;
    if (inforTypeId == Ext.IIPS.EnumInforType.InforCard.toString()) {
        url = fileType + "|" + url + "?0|0|" + fileInforId;
    }
    Ext.IIPS.InformationFileRender.ViewDetailFile(title, tab, depth, url, index, frame);
}

Ext.IIPS.InformationFileRender.FileDetailSendFile = function (fileType, fileInforId, fileSendId, fileReceiveId, fileBussinessId, inforTypeId, ViewDes) {
    var url = "";
    var title = "";
    var frame = "/Web/InformationCenter/InformationCenterFrame.aspx";
    var index = 0;

    switch (inforTypeId) {
        case Ext.IIPS.EnumInforType.GAQuickReport.toString():
            title = "(FW)公安信息快报";
            url = "Bulletin/BulletinEdit.aspx";
            index = 100;
            break;
        case Ext.IIPS.EnumInforType.AlarmChange.toString():
            title = "(FW)警情移交登记表";
            url = "AlarmChange/AlarmChangeEdit.aspx";
            index = 101;
            break;
        case Ext.IIPS.EnumInforType.AlarmPropertyChange.toString():
            title = "(FW)警情性质变更申请表";
            url = "AlarmChangeProperty/AlarmPropertyChangeEdit.aspx";
            index = 102;
            break;
        case Ext.IIPS.EnumInforType.Investigation.toString():
            title = "(FW)协查登记表";
            url = "Investigation/InvestigationChangEdit.aspx";
            index = 103;
            break;
        case Ext.IIPS.EnumInforType.InvestigationRevoke.toString():
            title = "(FW)撤销协查登记表";
            url = "Investigation/Revoke.aspx";
            index = 104;
            break;
        case Ext.IIPS.EnumInforType.InforCard.toString():
            title = "(FW)信息卡";
            frame = "/Web/InformationCenter/EventsFrame.aspx";
            url = "Events/EventDetail.aspx";
            index = 105;
            break;
         
        case Ext.IIPS.EnumInforType.NoTemplate.toString():
            title = "(FW)发文登记";
            url = "NoTemplate/NoTemplateEdit.aspx";
            index = 106;
            break;
    }
    url = fileType + "|" + url + "?" + fileInforId + "&" + inforTypeId;
    if (inforTypeId == Ext.IIPS.EnumInforType.InforCard.toString()) {
        url = fileType + "|" + url + "?0|0|" + fileInforId;
    }
    title = ViewDes != null ? (ViewDes + title) : title;
    Ext.IIPS.InformationFileRender.ViewDetailFile(title, url, index, frame);
 
}

Ext.IIPS.InformationFileRender.FileDetailReceiveFile = function (fileType, fileInforId, fileSendId, fileReceiveId, fileBussinessId, inforTypeId, sourceType, ViewDes) {
    var url = "";
    var title = "";
    var frame = "/Web/InformationCenter/InformationCenterFrame.aspx";
    var index =0;

    switch (inforTypeId) {
        case Ext.IIPS.EnumInforType.GAQuickReport.toString():
            title = "(SW)公安信息快报";
            url ="Bulletin/BulletinDetail.aspx";
            index = 100;
            break;
        case Ext.IIPS.EnumInforType.AlarmChange.toString():
            title = "(SW)警情移交登记表";
            url ="AlarmChange/SelectDetail.aspx";
            index = 101;
            break;
        case Ext.IIPS.EnumInforType.AlarmPropertyChange.toString():
            title = "(SW)警情性质变更申请表";
            url ="AlarmChangeProperty/SelectDetail.aspx";
            index = 102;
            break;
        case Ext.IIPS.EnumInforType.Investigation.toString():
            title = "(SW)协查登记表";
            url ="Investigation/SelectDetail.aspx";
            index = 103;
            break;
        case Ext.IIPS.EnumInforType.InvestigationRevoke.toString():
            title = "(SW)撤销协查登记表";
            url ="Investigation/SelectDetail.aspx";
            index = 104;
            break;
        case Ext.IIPS.EnumInforType.InforCard.toString():
            title = "(SW)信息卡";
            url ="Events/SelectDetail.aspx";
            index = 105;
            break;
     
        case Ext.IIPS.EnumInforType.NoTemplate.toString():
            title = "(SW)发文登记";
            url ="NoTemplate/NoTemplateDetail.aspx";
            index = 106;
            break;
        case Ext.IIPS.EnumInforType.Fax.toString():
            title = "(SW)传真";
            url = "Fax/FaxInforDetail.aspx";
            index = 107;
            break;
    }
    
  
    if (sourceType == 1)   //来源于传真
    {
        url ="Fax/FaxInforDetail.aspx";
        inforTypeId = Ext.IIPS.EnumInforType.Fax;
    }
     
    url = fileType + "|" + url + "?" + fileInforId + "&" + inforTypeId + "&" + fileBussinessId + "&" + fileReceiveId;
    title = ViewDes != null ? (ViewDes + title) : title;
    Ext.IIPS.InformationFileRender.ViewDetailFile(title,url, index, frame)
}

Ext.IIPS.InformationFileRender.ViewDetailFile = function (title, url, index, frame) {
    var tab = parent.ExtPageFrame.tabCenter;
    var depth = 2;
    if (tab == null) {
        tab = parent.parent.ExtPageFrame.tabCenter;
        depth = 3;
    }
    if (tab == null && url != "") {

        parent.parent.Ext.IIPS.FrameFun.RelationWindow(title, frame + "?" + url);

    }
    else {
        if (depth == 2) {
            parent.ExtPageFrame.addTab(title, url, index, frame, "Web/InformationCenter/InformationView.aspx", false);
        }
        else {
            parent.parent.ExtPageFrame.addTab(title, url, index, frame, "Web/InformationCenter/InformationView.aspx", false);
        }
    }
}
