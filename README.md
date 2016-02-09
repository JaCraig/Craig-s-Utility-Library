### Be the Batman of Programmers

[![Join the chat at https://gitter.im/JaCraig/Craig-s-Utility-Library](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/JaCraig/Craig-s-Utility-Library?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Have you ever thought about why Batman is as effective as he is? I mean sure he's rich, in better shape than any of us will ever be, and is a master at martial arts, but what almost always wins the day for him is that utility belt. It's the never ending supply of gadgets and utilities that he can use in the appropriate situation that really saves the day. And Craig's Utility Library is the same for programmers.

With .Net we have a number of built in classes and functions to help make a programmer's life easier, but let's face facts, they didn't think of everything. Craig's Utility Library tries to fill in some of those gaps (or at least the ones that I've run into). It comes with a couple hundred extension methods, built in data types such as a BTree, priority queue, ring buffer, etc. And that's just the DataTypes namespace. When you add it all up, Craig's Utility Library is one of the largest set of utilities for .Net out there...

OK, enough of the self aggrandizing rhetoric. The library is rather large so I'm going to break this down in the [wiki](https://github.com/JaCraig)

### IoC container
At the heart of the latest version of Craig's Utility Library is the IoC container. Unlike versions past, the system is designed to allow 3rd party libraries to be used instead of the built in components. This includes the IoC container itself which can be replaced with any container you prefer.

The IoC container can be accessed at any time using the following (note that in MVC, the IoC container is automatically added as the dependency resolver):
```
Utilities.IoC.Manager.Bootstrapper
```
This object is used by the rest of the library to set up the various pieces. The system contains a simple internal IoC container (similar to Funq) that it uses by default. You can, however, use your own within the system. In order to do this simply implement the Utilities.IoC.Interfaces.IBootstrapper interface. There is also a base class, Utilities.IoC.BaseClasses.BootstrapperBase that can be used to help implement the interface.

When attaching your preferred system, note that exceptions should not be thrown. All exceptions should be caught, logged, and the default value specified by the user should be returned. If the type is not registered, for instance, the default value should be returned and no exception should be thrown. If you would like an example implementation, please look at the Utilities.IoC.Default.DefaultBootstrapper class.

When building plugins for the system, you will need to register your types with the IoC container. In order to do this, simply create a class that implements the Utilities.IoC.Interfaces.IModule interface. This interface has an Order property (telling it when to run, lower numbers run first) and a Load function. The load function will be called at application start and will feed you the bootstrapper object. You should be able to use this to register your types.

Ex:
``` 
    /// <summary>
    /// File system module
    /// </summary>
    public class FileSystemModule : IModule
    {
        /// <summary>
        /// Order to run it in
        /// </summary>
        public int Order
        {
            get { return 0; }
        }

        /// <summary>
        /// Loads the module
        /// </summary>
        /// <param name="Bootstrapper">Bootstrapper to register with</param>
        public void Load(IBootstrapper Bootstrapper)
        {
            Bootstrapper.Register(new Manager());
        }
    }
```

Above is the basic file system registration module. In the case above the Manager object that is created will be returned every time an object of that type is requested. If a new object was desired, using the generic versions of the Register function would accomplish this.

### DataTypes
The DataTypes namespace contains a number of helpful data types (such as a BTree, Bag, RingBuffer, etc.) that are used internally by the system but are also available to anyone using the library. It also contains a number of sub systems that are used by various namespaces. For instance it comes with a simple AOP system that uses Roslyn. It in turn is used by the ORM to implement lazy loading, change tracking, etc. It comes with a simple conversion system that will convert most types built in to .Net into another. And if it can't, it allows you to build your own converter. There are dynamic type base classes that are used in various systems, items to help with threading, etc. The big stuff is the extension methods. There are over 100 extension methods for things like IEnumerable<T>, string, DateTime, TimeSpan, byte[], Exception, arrays, Func, Action, ICollection, IComparable, Stream, etc. Just add:

```
using Utilities.DataTypes
```

And you're good to go. This namespace is probably the most generally helpful.

### DataTypes - Dynamo
The DataTypes namespace comes with a class called Dynamo. This class adds a more robust dynamic object to .Net. The reason for this type is simply that dynamic in .Net isn't really dynamic. This pretty much limited my use of the keyword. I mean there is ExpandoObject which I've used a bunch but mostly for serialization situations. However you can't assign an ExpandoObject to a random type and expect it to convert properly. The Dynamo object, on the other hand, will do this:

```
public class ExampleClass
{
	int A { get; set; }
	string B { get; set; }
}

...

dynamic Object1=new Dynamo(new { A = 10, B="Example" });
ExampleClass Object2=Object1;
```

The above code will create an ExampleClass object with A equal to 10 and B equal to "Example" and assign that to Object2. Dynamo also has the ability to add functionality using extensions and has change tracking built in. If you're looking for something to compare it to, it's closest to Oak.  If you've ever used Oak, it's similar but is more of an attempt to get duck typing into .Net and not prototyping.

### DataTypes - AOP
If you use the Dynamo class, you may also be interested in the AOP system. The AOP system allows you to write code that can be added to another class at run time. It does this by creating a child class that derives from the class you specify. At start up, the system will generate these sub classes based on the aspects that you specify. By default there is only the ORM aspect built into the system. This adds lazy loading support. However this system is also used by the Dynamo class. Whenever Dynamo converts to another class type, it attempts to use the AOP system to generate the class. This means that the ExampleClass object above in reality will return an ExampleClassDerived type. This means that you can add any cross cutting code into your types easily.

### DataTypes - Conversion
One of the nice things with the addition of Dynamo is that a rather robust conversion system was needed. This system lets you convert from one type to another without much thought. Simple types such as int, float, string, enum, etc. are handled easily. More complex types it will attempt to copy all properties from one side to the other. It handles most cases (going from a COM object to managed may throw it off, but all managed classes should be simple enough). In order to tie into the system all you need to do is call the To extension method. This is a generic extension that attaches to all objects:

```
MyTestClass TestObject = new MyTestClass();
ExpandoObject Object = TestObject.To<MyTestClass, ExpandoObject>();
```

The above converts from a MyTestClass to an ExpandoObject. Not the most advanced code in the library, but it comes in quite handy especially when dealing with objects of an unknown type.

### DataTypes - DataMapper
The above system (the conversion system) is made possible with the data mapper that is built in. It's similar to AutoMapper (minus some functionality) but has the added benefit of being able to map functions to properties. It's mostly used in the back end but can be tied into.


### IO - File System
The IO namespace is divided into a couple different items. The first is the file system. One of the many issues with .Net is the fact that there are 20 different ways to do the same (or similar) thing. IO falls under that category sadly. FileInfo, VirtualFile, StorageFile, etc. Why can't anyone make up their minds on this stuff? Anyway, the system has a extendable wrapper around the file system that can access not just local files but any sort of file system that you build a plugin for. For instance, it comes with a local file system, relative, and web. It can access C:\ and www.google.com in the same manner. Plus it's extensible so if you want to Sky Drive, Dropbox, Google Drive, etc. then you can fairly easily.

Accessing the system uses two classes from the user side of things: Utilities.IO.FileInfo and Utilities.IO.DirectoryInfo. These classes take care of all initialization, figuring out how to handle the path passed in, etc. For instance reading from a file would look like this:

```
string Content=new FileInfo("~/Test.txt").Read();
```

The ~ symbol is available on both windows and web applications and points to the base directory for the application (note that if you implement a virtual file system in ASP.Net, it will use that instead). You can also do the following:

```
string Content=new FileInfo("http://www.google.com").Read();
```

And it will read the content from the website specified. The FileInfo and DirectoryInfo classes will attempt to parse anything passed in but note that complex paths may confuse it at present but it works for most paths. There are also currently some limitations for websites but this is currently being expanded on along with FTP, etc.

If you wish to add your own file system as an extension, all you need to do is create a class that inherits from Utilities.IO.FileSystem.Interfaces.IFileSystem. This interface has only 1 property and 3 functions that need to be implemented. You may, however, have to also implement both a file implementation (which would inherit from Utilities.IO.FileSystem.Interfaces.IFile) and a directory implementation (Utilities.IO.FileSystem.Interfaces.IDirectory). There are implementations of these under Utilities.IO.FileSystem.Default and base classes under Utilities.IO.FileSystem.BaseClasses that may help with this process. Note that the base classes have a great deal of extra functionality built in, so definitely give them a look.

### IO - Serialization
The serialization portion of the IO namespace, from a user standpoint is rather simple. By including Utilities.IO, the extension methods Serialize and Deserialize will be added. These extension methods allow you to access the various back end serializers registered with the system by either using the appropriate MIME type or the built in SerializationType class. Using the methods would look like this:

```
string SerializedData=new ExampleClass(){A=100,B="Hello world"}.Serialize<string,ExampleClass>();
```

The example above would serialize the class as JSON using the default DataContractJsonSerializer built into .Net. You can also specify the MIME type of the object, the built in serializers use the following MIME types:
* JSON: application/json
* SOAP: application/soap+xml
* XML: text/xml
* Binary: application/octet-stream

JSON is simply the default that the system uses. These all use the built in serializers from .Net. If you would like to use your own (such as JSON.Net or ServiceStack.Text) you can easily do so by simply making a class that implements the Utilities.IO.Serializers.Interfaces.ISerializer<T> interface. So if you write a JSON replacement, the system will pick that up and use that instead. You can also write serializers of other types not covered by the library (custom file format, etc) and it will find those as well.

### IO - Logging
The logging code is extremely simple from the end user's standpoint but a little more complicated from the implementor's standpoint. From an end user standpoint there is only one class/function that they need to worry about:

```
Utilities.IO.Log.Get("LogName");
```

The function simply gets a log object based on a name. If one does not exist, it creates it on the fly. This function returns an ILog object. This object has one function that matters, LogMessage:

```
void LogMessage(string Message, MessageType Type, params object[] args);
```

This method takes the message text, type of message (Info, General, Debug, etc), and a number of args which are combined with the text using string.Format (at least in the built in logger it is, custom implementations can treat those values differently). The default logger built in to the library is very basic and is not customizable. It simply defaults to writing a file at either ~/Logs or ~/App_Data/Logs depending on whether it is a web application or not. The name of the file will be of the format LogName+"-"+DateTime.Now.ToString("yyyyMMddhhmmss").

In order to implement your own logger of choice, simply implement both the Utilities.IO.Logging.Interfaces.ILogger and Utilities.IO.Logging.Interfaces.ILog interfaces. The ILogger interface is the actual logging engine while ILog is the object used by the system to actually log the info.

### IO - Messaging
By messaging, I mean things like email, etc. This system is the most in flux at the moment but is still rather simple. Generally the system consists of a message type, a message system, and formatters. The basic message type that comes with the IO namespace is EmailMessage. By itself it does very little. It holds data and that's about it. The system also comes with a message system that handles EmailMessage classes, SMTPSystem. This system actually sends the email. From an end user standpoint though, they only ever see the EmailMessage class:

```
using(EmailMessage Message=new EmailMessage())
{
    Message.To="Example@gmail.com";
	Message.From="MyEmail@companyname.com";
	Message.Subject="Subject";
	Message.Body="Body text";
	Message.Send();
}
```

At present there is only the ability to send messages. I would like to change this so that they can be received as well. So it's a bit in flux but it works for now. In order to implement a message/messaging system the key interfaces are Utilities.IO.Messaging.Interfaces.IMessage and Utilities.IO.Messaging.Interfaces.IMessagingSystem. I also mentioned formatters. Formatters are currently not implemented within the system itself but it allows you to specify how a message should be formatted before it is sent out. For instance if you wanted to use Razor to format the email message, you could. However I'm still playing with this so I'm not going to show any code currently.

### IO - File Formats
CUL has a number of built in file formats that it can read and output to: RSS, Excel, INI, VCalendar, VCard, CSV (or any other delimited file), fixed length files, etc. At present there is no common interface for these but that will probably come in some future update. At present though these helper classes can be found in Utilities.IO.FileFormats.

### IO - Compression and Encryption
If you've used the encryption and compression extension methods and classes from 3.4, not much has changed. Both have been simplified and moved into the IO namespace. There is a bit of work on the back end to make it easier to patch in your own algorithms but otherwise it works about the same.

### Configuration
CUL comes with a simple system to help with getting information out of various configuration files. For instance, it can get information from the web.config or app.config files (it uses that by default). It also has files to help with XML based or JSON based config files.

### Media
Years ago I was interested in 2D and 3D graphics. As such I copied a number of old C++ classes that I had created back then and built them into a series of extension methods to help with manipulating images. It contains things such as color to black and white, sharpen, box blur, bump map generation, dilation, edge detection, embossing, etc. It's not extremely fast on large images but if you just need something quick, it will work.

### ORM
On top of everything else, CUL has a built in ORM. It's still a work in progress and only supports SQL Server currently. However the system has been reworked in such a way that adding different databases or other items, such as AD or WMI, will be possible. Also note that if you were using SQLHelper before, this has been removed. Instead a QueryProvider static class has been added. This class contains a function called Batch. This returns an IBatch type that is used to make combining multiple queries together into one much simpler than before:
```
IList<IList<dynamic>> Results = Utilities.ORM.QueryProvider.Batch("ConnectionString")
                                                     .AddCommand(null, null, CommandType.Text, "SELECT * FROM Table")
                                                     .AddCommand(null, null, "SELECT * FROM Table WHERE ID=@0", CommandType.Text, 10)
                                                     .Execute();
```
Results in the above code is a list that contains the results from both queries. Each of these results is a list of Dynamo objects which can then be converted to whatever type you need. The Batch command takes either the connection string itself or the name of a connection string from the config file for the application. Note that this is still a work in progress and the interfaces may change as I decide certain things are too difficult or not needed.

### Profiler
Another system built in to the library is the profiler. By default a simple profiler is provided but other libraries can be substituted such as MiniProfiler. Using the profiler is rather simple:
```
using(Utilities.Profiler.Profiler ExampleObject=new Utilities.Profiler.Profiler("FunctionName"))
{
	//Code you want to profile
}
```
You can then get the profiler information by calling ToString on the Manager class (Utilities.Profiler.Manager.Manager).

### Random
The Random namespace is designed to be a bit more robust than the built in System.Random class. Basically it adds the ability to do randomization of names, towns, states, zip codes, phone numbers, email addresses, dates, etc. Basically it's a ton of extension methods and attributes that you can use to help you with randomization of entire classes:
```
System.Random Rand = new System.Random();
RandomTestClass Item = Rand.NextClass<RandomTestClass>();
```
You have to use the attributes that come in the Utilities.Random namespace, but it can help quite a bit with testing.

### Validation
The validation namespace is really just a collection of ValidationAttribute classes. They add basic functionality such as determining if a value is between two values, comparing the value to another property, etc. Nothing too advanced, helpful but rather simple.

### Web
Web has been slimmed down quite a bit. Some items have been moved into IO and DataTypes. Currently it only holds the extension methods for HttpContext, HttpRequest, etc. 

### Documentation
The library itself is documented and comes with the XML generated docs. There is also a download available on Nuget that contains the documentation generated using doxygen (http://www.stack.nl/~dimitri/doxygen/). With version 4.0, the basic info to get you started can be in the document above. Feel free to ask specific questions on the CodePlex (http://cul.codeplex.com) or Github (https://github.com/JaCraig/Craig-s-Utility-Library) pages also.

### Where To Get It
The library is available on NuGet in both a single DLL as well as each individual namespace.

**Entire library (with command from package manager console):**
* Craig's Utility Library: Install-Package CraigsUtilityLibrary

**Individual namespaces (with command from package manager console):**
* DataTypes: Install-Package CraigsUtilityLibrary-DataTypes
* Validation: Install-Package CraigsUtilityLibrary-Validation 
* Media: Install-Package CraigsUtilityLibrary-Media 
* Web: Install-Package CraigsUtilityLibrary-Web 
* ORM: Install-Package CraigsUtilityLibrary-ORM 
* Profiler: Install-Package CraigsUtilityLibrary-Profiler 
* Configuration: Install-Package CraigsUtilityLibrary-Configuration 
* Random: Install-Package CraigsUtilityLibrary-Random 
* IO: Install-Package CraigsUtilityLibrary-IO 
* IoC: Install-Package CraigsUtilityLibrary-IoC

### Contributors/Contributing
Maintained by [James Craig](https://github.com/JaCraig)

If you have a method, fix, or change that you would like added the best option is to: 
* Fork the codebase 
* Create a branch 
* Commit your changes 
* Push to the branch 
* Open a pull request.

### License

Uses MIT license