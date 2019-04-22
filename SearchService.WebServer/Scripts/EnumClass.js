Ext.IIPS.EnumInforType = {      //模板类型
    GAQuickReport: 1, /*公安信息快报*/
    AlarmChange: 2, /*警情更改登记表*/
    AlarmPropertyChange: 3, /*警情性质变更申请表*/
    Investigation: 4, /*协查登记表*/
    InvestigationRevoke: 5, /*撤销协查登记表*/
    InforCard: 6, /*信息卡*/
    Fax: 7, /*传真*/
    NoTemplate:999  /*无模板文件*/
}

Ext.IIPS.EnumFileType = {
    NewFile: 0, /*新文件-未上报或分发文件*/
    SendFile: 1, /*发文*/
    ReceiveFile: 2, /*收文*/
    InforCardFile: 3, /*信息卡*/
    GetFileTypeName: function (typeValue) {
        switch (typeValue) {
            case Ext.IIPS.EnumFileType.NewFile:
                return "新文件";
            case Ext.IIPS.EnumFileType.SendFile:
                return "发文";
            case Ext.IIPS.EnumFileType.ReceiveFile:
                return "收文";
            case Ext.IIPS.EnumFileType.InforCardFile:
                return "信息卡";
        }
    }
}

Ext.IIPS.EnumEventSourceType = {
    None: 0, /*无来源*/
    Call:1,/*来自电话*/
    SMS: 2, /*来自短信*/
    Alarm: 3, /*来自重大警情*/
    Bulletin: 4, /*信息快报*/
    GetEventSourceType: function (typeValue) {
        switch (typeValue) {
            case Ext.IIPS.EnumEventSourceType.None:
                return "无来源";
            case Ext.IIPS.EnumEventSourceType.Alarm:
                return "警情";
            case Ext.IIPS.EnumEventSourceType.SMS:
                return "短信";
            case Ext.IIPS.EnumEventSourceType.Bulletin:
                return "快报";
            case Ext.IIPS.EnumEventSourceType.Call:
                return "电话";
        }
    }
}

Ext.IIPS.EnumFileDocStatus = 
{
    /// <summary>
    /// 草稿
    /// </summary
    Drafting:1,
    /// <summary>
    /// 审批中
    /// </summary>
    Auditing:2,
    /// <summary>
    /// 已呈市局领导
    /// </summary>
    SentToLead : 3,
    /// <summary>
    /// 已转来文
    /// </summary>
    Relayed : 4,
    /// <summary>
    /// 局领导已批示
    /// </summary>
    LeadInstructed : 5,
    /// <summary>
    /// 审批完成
    /// </summary>
    Audited : 6,
    /// <summary>
    /// 已发送
    /// </summary>
    Sent: 7,
    GetFileDocStatus: function (typeValue) {
        switch (typeValue) {
            case Ext.IIPS.EnumFileDocStatus.Drafting:
                return "草稿";
            case Ext.IIPS.EnumFileDocStatus.Auditing:
                return "审批中";
            case Ext.IIPS.EnumFileDocStatus.SentToLead:
                return "已呈市局领导";
            case Ext.IIPS.EnumFileDocStatus.Relayed:
                return "已转来文";
            case Ext.IIPS.EnumFileDocStatus.LeadInstructed:
                return "局领导已批示";
            case Ext.IIPS.EnumFileDocStatus.Audited:
                return "审批完成";
            case Ext.IIPS.EnumFileDocStatus.Sent:
                return "已发送";
        }
    }
}
Ext.IIPS.EnumFaxPriority = {        //传真发文优先级
    High: 1,      //高
    Medium: 2,    //中
    Low:3         //低
}
Ext.IIPS.EnumFaxSendType = {        //传真发送类型
    OnlyFiles: 0,      //仅发原件
    NeedRecept: 1,    //需要回执
    HanleFile: 2,         //需要处理表
    NeedReceptAndHanleFile:3    //需要回执和处理表
}
Ext.IIPS.EnumFaxSendCallRecept = {        //传真发送是否需要语言催回执
    No: 0,      //否
    Yes: 1      //是
}

Ext.IIPS.FileSendType = {    //文件发送类型   
    Unit: 0,
    Person: 1,
    Mobile: 2,
    Telephone: 3,
    Fax: 4,
    AlarmBell: 5
}

Ext.IIPS.FileDealInforType = {    //文件处理类型   
    WebFile: 0,
    SMS: 1,
    AlarmBell: 2
}

//用户类型
Ext.IIPS.UserType = {
    Unit: 0,
    Person:1
}

//单位类型
Ext.IIPS.UnitType = {
    Unit: 0,
    Duty: 1
}

//值班角色
Ext.IIPS.DutyRoleType = {
    DutyLeader1: 1,//主班局领导
    DutyLeader2: 2,//副班局领导
    DutyZHZ: 3,//指挥长
    DutyCZ: 4,//值班处长
    DutyKZ: 5,//值班科长
    DutyZBY: 6//值班民警
}

Ext.IIPS.FileOnline = "http://192.168.0.44:10088/";
Ext.IIPS.Officeline = "http://192.168.0.44/";
Ext.IIPS.WebSocket = 'ws://192.168.0.50:2020';
Ext.IIPS.DelaySecond = 1000;      //延时重复执行操作
Ext.IIPS.DelayMinute = 20;      //用户未操作时间   webconfig: <sessionState mode="InProc" customProvider="DefaultSessionProvider" timeout="20> session超时时间