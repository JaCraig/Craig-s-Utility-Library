### Be the Batman of Programmers
Have you ever thought about why Batman is as effective as he is? I mean sure he's rich, in better shape than any of us will ever be, and is a master at martial arts, but what almost always wins the day for him is that utility belt. It's the never ending supply of gadgets and utilities that he can use in the appropriate situation that really saves the day. And Craig's Utility Library is the same for programmers.

With .Net we have a number of built in classes and functions to help make a programmer's life easier, but let's face facts, they didn't think of everything. Craig's Utility Library tries to fill in some of those gaps (or at least the ones that I've run into). It comes with a couple hundred extension methods, built in data types such as a BTree, priority queue, ring buffer, etc. And that's just the DataTypes namespace. When you add it all up, Craig's Utility Library is one of the largest set of utilities for .Net out there.

### Getting Started
There is a ton of code in the library and it may be difficult to figure where to start. The items that find are almost always useful for everyone is the DataTypes and IO namespaces. The IO namespace makes reading/writing files much simpler, deals with serialization, and has a simple logging framework. The DataTypes namespace, on the other hand is where a lot of the extension methods are located for string manipulation, DateTime helpers, ICollection, IEnumerable, Arrays, Dictionarys, etc. On top of that, there are a few data type classes here that can help quite a bit.
```
string Example="5555551010";
Example=Example.FormatString("(###)###-####");  //Formats the string as (555)555-1010
```

Or reading a file:
```
string Content=new FileInfo("MyFile.txt").Read(); //Reads the file to the end
```

From there you can branch out into FileFormats, SQL, Validation, Reflection, Encryption, Compression, and Random.

**FileFormats namespace**
FileFormats contains a couple of CSV/Delimited file parsers, BlogML helper classes, fixed length file helpers, INI parser, RSS helper, iCal/vCal helper, vCard helper, a simple zip file creator, as well as other formats. They're not the most advanced but are designed to get you at the data that you want with minimal fuss:
```
CSV ExampleCSV=new CSV(CSVContentString);
string Value=ExampleCSV[RowNumber][ColumnNumber].Value;
```
SQL contains one of the easiest wrappers for ADO.Net out there:
```
using(SQLHelper Helper=new SQLHelper("SELECT * FROM MyTable",System.Data.CommandType.Text,"DatabaseConnection"))
{
   Helper.ExecuteReader();
   while(Helper.Read())
   {
      int Value1=Helper.GetParameter("Value1",0);
	  string Value2=Helper.GetParameter("Value2","");
   }
}
```
Or you can even use the built in MicroORM for that class. It doesn't have the speed of Massive or Dapper. It's between PetaPoco and BLToolkit.

**Validation namespace**
The Validation namespace builds on the DataAnnotations namespace adding in the ability to check if a property is between two values, compare to a static value, compare to another property, if an IEnumerable contains a value, if an IEnumerable is empty, the ability to check if a property is a valid credit card number, a decimal, a domain, and an integer. Other attributes are included.

**Random namespace**
The Random namespace contains extension methods and attributes to help randomly generate basic built in types as well as classes. You can generate names, companies, cities, addresses, domain names, email addresses, phone numbers, etc. all using the classes included. Want to generate 1000+ users for your test database? Extremely easy using the Random namespace.

**Encryption and Compression namespaces**
The Encryption and Compression classes contain very easy to use extension methods for encryption and compression respectively. Want to do an SHA1 or RSA256 hash of that string? Just call the Hash extension and tell it which HashAlgorithm class to use and it will do the rest. Want to compress a string using GZip? Same thing, one extension method and it handles everything for you.

**Reflection namespace**
The Reflection namespace on the other hand has a bunch of extensions that you may not have a need for immediately but are extremely useful.
```
IEnumerable<IMyInterface> Classes=new DirectoryInfo("./MyAssemblies/").GetObjects<IMyInterface>();
```
The above code loads any DLLs found in the MyAssemblies directory and creates an instance of any classes that use the IMyInterface interface (that have a default constructor). I find it a bit easier than using something like MEF, but everyone has their opinion.

**Other namespaces**
The library also contains: 
* AI namespace (which is really only useful for the NaiveBayes class)
* Caching namespace (which contains a thread safe cache for small projects as well as some extension methods to make caching data easier)
* Configuration namespace (think web.config but easier to deal with)
* DataMapper namespace (similar to AutoMapper)
* Environment namespace (which... well, isn't really even close to being done but it's there)
* IoC namespace (really basic IoC container)
* LDAP namespace (very easy to use set of classes that let you connect to AD/LDAP and do queries)
* Math namespace (helpful set of math related classes and extension methods)
* Media namespace (which has one of the larger set of extension methods when doing image manipulation including a nice bitmap to ASCII art method)
* ORM namespace (a larger, more full featured ORM built on top of the micro ORM)
* Profiler namespace (good for small bits of code that you would like to time/profile)
* Web namespace (if it's web related, it's probably in there).

And if I find something interesting, or want to explore a space of programming, or even just find that I'm writing the same bits of code more than once, I will probably add it.

### Documentation
The library itself is documented and comes with the XML generated docs. There is also a download available on CodePlex (http://cul.codeplex.com/) that contains the documentation generated using doxygen (http://www.stack.nl/~dimitri/doxygen/). The library itself is slightly too large and changes too frequently (with additions, etc) for me to write detailed documentation but I am going back through the code and adding basic examples currently.

### Where To Get It
The library is available on NuGet in both a single DLL as well as each individual namespace.

**Entire library (with command from package manager console):**
* Craig's Utility Library: Install-Package CraigsUtilityLibrary

**Individual namespaces (with command from package manager console):**
* DataTypes: Install-Package CraigsUtilityLibrary-DataTypes
* LDAP: Install-Package CraigsUtilityLibrary-LDAP
* SQL: Install-Package CraigsUtilityLibrary-SQL
* Encryption: Install-Package CraigsUtilityLibrary-Encryption 
* Caching: Install-Package CraigsUtilityLibrary-Caching
* Math: Install-Package CraigsUtilityLibrary-Math
* Validation: Install-Package CraigsUtilityLibrary-Validation 
* Environment: Install-Package CraigsUtilityLibrary-Environment
* Media: Install-Package CraigsUtilityLibrary-Media 
* Web: Install-Package CraigsUtilityLibrary-Web 
* ORM: Install-Package CraigsUtilityLibrary-ORM 
* Compression: Install-Package CraigsUtilityLibrary-Compression 
* Profiler: Install-Package CraigsUtilityLibrary-Profiler 
* FileFormats: Install-Package CraigsUtilityLibrary-FileFormats 
* Configuration: Install-Package CraigsUtilityLibrary-Configuration 
* Random: Install-Package CraigsUtilityLibrary-Random 
* DataMapper: Install-Package CraigsUtilityLibrary-DataMapper 
* IO: Install-Package CraigsUtilityLibrary-IO 
* Reflection: Install-Package CraigsUtilityLibrary-Reflection
* AI: Install-Package CraigsUtilityLibrary-AI 
* IoC: Install-Package CraigsUtilityLibrary-IoC 

**Old namespaces (with command from package manager console):**
* Cisco: Install-Package CraigsUtilityLibrary-Cisco
* Error: Install-Package CraigsUtilityLibrary-Error 
* Classifier: Install-Package CraigsUtilityLibrary-Classifier 
* Multithreading: Install-Package CraigsUtilityLibrary-Multithreading 
* Events: Install-Package CraigsUtilityLibrary-Events 
* CodeGen: Install-Package CraigsUtilityLibrary-CodeGen 
* Exchange: Install-Package CraigsUtilityLibrary-Exchange 

### Contributors/Contributing
Maintained by [James Craig](https://github.com/JaCraig)

If you have a method, fix, or change that you would like added the best option is to:
1. Fork the codebase
2. Create a branch
3. Commit your changes
4. Push to the branch
5. Open a pull request.

### License
Uses MIT license