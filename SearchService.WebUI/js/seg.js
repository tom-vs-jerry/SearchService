$(document).ready( function(){
	var Searching=false;
	var inputKey ="";
	var oldinputKey="";
	var SearchTime=new Date();
	var socket = new WebSocket('ws://127.0.0.1:2020');
	var inputKey ="";
	var pageSize = 5;	

	//生成
	function setPageControl(page_no,visible_count_of_page,count_of_page)
	{
		$('#result_pagination').bootpag({
			total: count_of_page,          			// total pages
			page: page_no,            				// default page
			maxVisible: visible_count_of_page,     	// visible pagination
			leaps: true,         					// next/prev leaps through maxVisible
			first: '<<',
			last:'>>',
			next:'>',
			prev:'<',
			firstLastUse:true
		}).on("page", function(event, num){
			clickPageButton(num,count_of_page);		
		});
	}

	function createResultHTML(results,idx)
	{
		item = results[idx];
		result_title = item.Title;
		result_time = item.Time.substr(0,10);
		result_segment = item.Summary;
		urls = item.Urls.split(";");
		
		result_content ='<div class="row result">'
		result_content += '<div class = "col-sm-12">'
		result_content += '<div class="result-title"><span>\
						<a class="result-title" href="'+urls[0]+ '" target="_bank" >'+result_title+'</a>';
												
		for(var i=0; i<urls.length; i++)
		{
			result_content += '<a href="'+ urls[i]+'" target="_bank"><img src="./images/y.png" style="padding-left:5px"/></a>';
		}			
		result_content += '</span><br><span class ="result-property">日期：'+result_time+'</span></div>';
		
		result_content += '<div class="result-segment"><span>\
				<a style="font-size:14px;color:blue;"  id="tba_6c4d8faf-9e79-4f5e-81ca-e1890cc4ea88"> 				【详情】</a>'+result_segment+ '</span></div>'	;
		result_content +='</div>';
		result_content += '</div></div></div>';			
		
		return result_content;
	}

	function ShowResult(jsondata)
	{
		//$("#spnTotal").html(json.data);
		//转化成对象
		var a = JSON.parse(jsondata.data);
		//$("#spn1").html("inforID>>R[1]="+a.R[1]);
		//$("#spn2").html("page size>>R[2]="+a.R[2]);
		//$("#spn3").html("page_no>>R[3]="+a.R[3]);
		//$("#spn4").html("count of result>>R[4]="+a.R[4]);
		//$("#spn5").html("Total of Page>>R[5]="+a.R[5]);
		//$("#spn6").html("R[0]="+a.R[0]);	
		//alert("OK");
		if (a.Q == "RI") {			
			var results= JSON.parse(a.R[0]);
			var inforID = a.R[1];
			var countofresult= parseInt(a.R[4]);		
			var	pageno = parseInt(a.R[3]);	
			var pagesize = parseInt(a.R[2]);	
			var totalcountofpage=parseInt(a.R[5]);
			
			$("#result_stat").text("找到 "+countofresult+" 条结果(用时 "+ (new Date()-SearchTime)/1000 +"秒)");
			result_html="";
			for(var i=0; i< results.length; i++)
			{											
				result_html += createResultHTML(results,i);														
			}
			$("#result_container").html(result_html);
					
			//设置总记录数、当前页码数
			if(countofresult>pagesize)
			{
				$('#result_pagination').show();
				setPageControl(pageno,pagesize,totalcountofpage);
			}
			else
			{
				$('#result_pagination').hide();			
			}
		}
	}

	function createCommand(input_key,page_no,page_size)
	{
		var startTime = "2000-01-01";//开始时间
		var endTime = "2000-01-01";
		var dateSort= 1
		var input = 'RI {"InPutWord":"' + input_key  + '","DateSort":"' + dateSort+ '","StartTime":"' + startTime + '","EndTime":"' + endTime + '","PageNo":' + page_no + ',"PageSize":' + page_size + '}';	
		return input;
	}

	function searchAsync(inputkey,pageno,pagesize)
	{		
		if(Searching && (new Date()-SeachTime)<3000)
		{		
			return;
		}
		//创建命令	
		inputKey = inputkey;
		command = createCommand(inputkey,pageno,pagesize);
		//alert(command);		
		//发送命令
		Searching=true;
		SearchTime=new Date();
		socket.send(command);	
		//socket接收数据事件
		socket.onmessage = function (json) {
			ShowResult(json);
			Searching=false;
		}
		socket.onerror = function (json) {
			alert("连接错误，请刷新网页重试！");
			Searching=false;
		}
		socket.onclose=function(){
			//alert("WebSocket colsed！");
			Searching=false;
		}
	}

	function clickPageButton(page_no, count_of_page)
	{	
		//alert(page_no);	
		searchAsync(inputKey, page_no, pageSize);
	}

	setPageControl(0,0,0)
	//提交按钮单击事件
	$('#submit').click(function () {	
		var inputKey = $('#iptInputWord').val();
		if (inputKey.trim() == "") 
		{
			alert("搜索框不能为空！");
			return;
		}	
		searchAsync(inputKey,1,pageSize);
	});

	$('#iptInputWord').bind('keydown', function (event) {  
		if (event.keyCode == 13) {  
			event.preventDefault();
			var inputKey = $('#iptInputWord').val();
			if (inputKey.trim() == "") 
			{
				alert("搜索框不能为空！");
				return;
			}	
			searchAsync(inputKey,1,pageSize);            
		}	
	})

	//联想功能
	$('#iptInputWord').autocomplete({
		source: function (request, response) {
			//获取输入数据
			var input = request.term.trim();
			if (input != '') {
				if (oldinputKey != input) {
					var input = 'SA {"Input":"' + request.term + '","Count":20}';
					socket.send(input);
				}
				oldinputKey = input;
			}
			socket.onmessage = function (json) {
				var a = JSON.parse(json.data);
				if (a.Q == "SA") {
					response(a.R);
				}
			}
		},
		select: function (event, ui) {
			$('#iptInputWord').val(ui.item.value);
			searchAsync(ui.item.value,1,pageSize);
		}
	});

})


