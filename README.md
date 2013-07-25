### IoC container
At the heart of the latest version of Craig's Utility Library is the IoC container. Unlike versions past, the system is designed to allow 3rd party libraries to be used instead of the built in components. This includes the IoC container itself which can be replaced with any container you prefer.

The IoC container can be accessed at any time using the following (note that in MVC, the IoC container is automatically added as the dependency resolver):
```
Utilities.IoC.Manager.Bootstrapper
```
This object is used by the rest of the library to set up the various pieces. The system contains a simple internal IoC container (similar to Funq) that it uses by default. You can, however, use your own within the system. In order to do this simply implement the Utilities.IoC.Interfaces.IBootstrapper interface. There is also a base class, Utilities.IoC.BaseClasses.BootstrapperBase that can be used to help implement the interface.

When attaching your preferred system, note that exceptions should not be thrown. All exceptions should be caught, logged, and the default value specified by the user should be returned. If the type is not registered, for instance, the default value should be returned and no exception should be thrown. If you would like an example implementation, please look at the Utilities.IoC.Default.DefaultBootstrapper class.

When building plugins for the system, you will need to register your types with the IoC container. In order to do this, simply create a class that implements the Utilities.IoC.Interfaces.IModule interface. This interface has an Order property (telling it when to run, lower numbers run first) and a Load function. The load function will be called at application start and will feed you the bootstrapper object. You should be able to use this to register your types.