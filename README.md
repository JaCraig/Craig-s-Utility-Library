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