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


ASP.NET Core appsettings.json文件
ASP.NET Core中的配置源
	appsettings.json,appsettings.{Environment}.json,不同环境下对应不同的托管环境
	
	User secrets(用户机密)
	Environment variables(环境变量)
	Command-line arguments(命令行参数)

访问配置信息
	IConfiguration 配置接口
我们可以在项目属性=>调试=>环境变量 开发环境

中间件:
-> Logging->StaticFiles->MVC->StaticFiles->Logging->
处理HTTP请求

1.可同时被访问和请求
2.可以处理请求后,然后将请求传递给下一个中间件
3.可以处理请求后,并使管道短路
4.可以处理传出响应
5.中间件是按照添加的顺序执行的


# Linux
命令解析器
	shell  --unix操作系统
	bash	--Linux操作系统
	本质: 根据命令的名字,调用对应的可执行程序

shell命令:
1.date  --当前时间
2.history	--显示出用户之前敲过的命令 
3.ctrl + p(等同于向上的箭头)  -- 在历史记录列表中做一个向上的滚动
4.ctrl + n(等同于向下的箭头)  -- 在历史记录列表中做一个向下的滚动
5.ctrl + b  -- 光标往前移动 
6.ctrl + f  -- 光标往后移动
7.ctrl + a  -- 光标移动到行首
8.ctrl + e	-- 光标移动到行尾
9.ctrl + h  -- 删除光标前边的字符
10.ctrl + d  -- 删除光标后边(覆盖)的字符、
11.ctrl + u  -- 删除光标前面所有的字符
12.tab	-- (一次)补全命令  (二次)智能提示 如果是路径的话,可以将下面的目录列出来
13.ls /  查看linux根目录  ("/" 表示根目录)

Linux系统目录结构:(树形结构,在linux上只有目录)
1> 根目录:
2> /bin bin是Binary的缩写,这个目录存放着最经常使用的命令
3> /dev dev是Device(设备)的缩写,该目录下存放的是Linux的外部设备，在Linux中访问设备的方式和访问文件的方式是相同的
4> /etc 这个目录用来存放所有的系统管理所需要的配置文件和子目录
5> /home 用户的主目录,在Linux中,每个用户都有一个自己的目录,一般该目录是以用户的账号命名的
6> /lib 这个目录里存放着系统最基本的动态连接共享库,其作用类似于Windows里的DLL文件,几乎所有的应用程序都需要用到这些共享库
7> /media linux系统会自动识别一些设备,例如U盘、光驱等等,当识别后,linux会把识别的设备挂载到这个目录下
8> /mnt 系统提供该目录是为了让用户临时挂载别的文件系统的,我们可以将光驱挂载在/mnt/上，然后进入该目录就可以查看光驱里的内容了
9> /root 该目录为系统管理员，也称作超级权限者的用户主目录
10> /usr
11> /boot 这里存放的是启动Linux时使用的一些核心文件,包括一些连接文件以及镜像文件（不能删除）
12> /lost+found 这个目录一般情况下是空的，当系统非法关机后,这里就存放了一些文件
13> /opt 这是给主机额外安装软件所摆放的目录,比如你安装一个ORACLE数据库则就可以放到这个目录下，默认是空的
14> /sbin s就是Super User的意思,这里存放的是系统管理员使用的系统管理程序
15> /usr 这是一个非常重要的目录,用户的很多应用程序和文件都放在这个目录下,类似与windows下的program files目录

用户目录
1>绝对路径:  从根目录开始写  /home/itcast/aa
2>相对路径:  bb相对于当前的工作目录而言
	. => 当前目录
	.. => 当前的上一级目录
	- => 在临近的两个目录直接切换 cd -
3>tg@VM-0-16-ubuntu tg:当前登录用户;@:at 在;VM-0-16-ubuntu:主机名;~:用户的家目录(宿主目录); 



