# NetCoreStudy

CreateDefaultBuilder() 执行的一些任务
	设置Web服务器
	加载主机和应用程序配置表信息
	配置日志记录
	
ASP.NET Core应用程序的托管形式
	在InProcess(进程内托管)或者OutOfProcess(进程外托管)
	
InProcess
	配置进程内托管
	<PropertyGroup>
    <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
    </PropertyGroup>
	
	1.在InProcess托管的情况下,CreateDefaultBuilder()方法调用UserIIS()方法并在IIS工作进程(w3wp.exe或iisexpress.exe)
	内托管应用程序
	生产环境 启用IIS w3wp.exe
	vs默认启用 iisexpress.exe
	
	2.从性能的角度来看,InProcess托管比OutOfProcess托管提供了更高的请求吞吐量
	
	3.获取执行应用程序的进程名称
		//获取当前进程的名称
		var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
		
什么是OutOfProcess托管
	有2个Web服务器 - 内部Web服务器和外部Web服务器
	内部Web服务器是Kestrel
	外部Web服务器可以是IIS,Nginx或Apache
	
什么是Kestrel Web Server?
	Kestrel是ASP.NET Core的跨平台Web服务器
	Kestrel本身可以用作边缘服务器 
	Kestrel中用于托管应用程序的进程是dotnet.exe
	
InProcess 进程内托管

Internet <=(Http)> IIS[w3wp.exe[Application]]
进程内托管
	>该应用程序托管在IIS工作进程中
	>只有一个Web服务器
	>从性能角度来看,在进程托管中,优于进程外托管
	
Out-of-Process 进程外托管
Kestrel可以用作面向Internet的Web服务器
Internet <=(Http)> Kestrel[dotnet.exe[Application]]

Kestrel还可以与反向代理服务器结合使用,例如IIS,Nginx或Apache
Internet <=(Http)>Reverse Proxy Server[IIS、Nginx、Apache]<=(Http)>Kestrel[dotnet.exe[Application]]



 进程内(InProcess)和进程外(out-of-Process)托管的对比
 进程内
	1.进程名称"w3wp.exe"或"iisexpress.exe"
	2.只有一个服务器
	3.性能更好
	
  进程外
	1.进程名称"dotnet.exe"
	2.两台服务器
	3.在内部和外部Web服务器之间代理请求的损耗
	
  launchSettings.json(本地)
  
  commandName 命令名称
  launchBrowser 是否进行浏览器加载
