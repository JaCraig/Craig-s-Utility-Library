Craig's Utility Library Contribution Guide
==========================================

** Bugs **

Be sure to search [existing issues](https://github.com/JaCraig/Craig-s-Utility-Library/issues) first. Include code example for reproducing the problem along with expected output. Also include your OS and version of .Net you are using if not using one of the NuGet packages (.Net Core, 4.6, Mono, etc).

** Feature Requests **

First, be sure to check the [feature backlog](https://trello.com/b/CLOTGldq/projects). You should be able to upvote features/additions that you would like prioritized. After that, add an item to the bug system on Github if it is not currently in the Trello board. 

** Submitting a Pull Request **

There are very few rules/guidelines but a number of "Rules of Thumb" when contributing to the project. The main items to keep in mind:

1. The library is MIT licensed. As such if the code that you are submitting is from another library that uses a stricter license (GPL, etc), then that code will be rejected outright. Many people have requested merging their code base into this one but have picked licenses that are not 100% compatible with MIT. This is the number one reason that code is rejected.
2. While extension methods, adding a wrapper for a third party library, etc. are pretty straightforward, if you plan to work on something fairly large then please contact me first. This allows me to give you feedback and let you know if I like the idea prior to you spending your valuable time on something that may not end up getting merged.
3. For the "Rules of Thumb" that may require some rewrite of your code when submitting, see below.
4. Most code contributions, if they're aimed at new code, should be on the next version release branch instead of the main branch. Bug fixes should be aimed at whichever branch the fix is for, obviously.

** Rules of Thumb For External/Internal Libraries **
* Create them as Shared Libraries (or whatever VS calls them now) when possible.
* Only use DNX or third party libraries that are compatible when possible.
* Reduce the number of libraries that you're using to the bare minimum.
* Always go through a round of simplification and look for code duplication.
* Have speed based tests for all third party libraries and internal types where appropriate. (The library uses [Sundial](https://github.com/JaCraig/Sundial) for this)
* Make sure compilation is possible cross platform. (So aim for .Net Core, etc.)
* Anything that is not .Net Core, needs to be in its own library that the system loads at run time.
* Signing the library no longer seems necessary unless there is a surge in SharePoint development. Also the library is moving towards .Net Core and away from the old GAC model. So unless things change, it can be skipped.
* Any database tests, use fake access for unit tests. Move actual tests against a database to its own test library (required in addition to the unit tests).
* All warnings should be treated as errors.
* Make sure to run static analysis of the code base and fix any/all errors that are found.

** Rules of Thumb For Classes **
* Constructors need to be IoC friendly.
* All functions must return a value. If it would normally be void, return the object itself.
* Where possible, make properties read only.
* Aim for 100% test coverage.
* Until contracts or something similar are added to the language, [Code Contracts](http://research.microsoft.com/en-us/projects/contracts/) is used by the library. Add all contracts that are suggested by the system unless it is a false positive.
* Types should be improved with appropriate operator overloading.
* Types should be improved with appropriate implicit conversion overloading.
* Add appropriate interfaces to types (IClonable, IComparable, etc).
* All objects should be thread safe.
* Where appropriate, make items async. (Especially IO or network based operations)
* All async calls should have Async as the suffix.
* Favor extension methods over static methods when public. Favor static methods over extension methods when private/internal.
* Comment all functions, properties, etc. with enough information that documentation can be generated from it.
* The basis of 5.0 is the same as 4.0, the IoC container. However instead of just using it as a service locator, it is used for everything. So make sure the class is IoC friendly.
* All parameters to a function should supply a default if null is passed in. In otherwords, always do null checks and act accordingly. Do not simply throw an exception or wait for one to happen in the code unless it is appropriate.

** Ways to Contribute **

There are many ways you can contribute to the project:

* Fix a bug or implement a new feature (see above).
* Write a wrapper for a third party library and let me know about it.
* Test CUL and report bugs you find.
* Write unit tests for CUL.
* Write documentation and help keep it up to date.

** Code of Conduct **

This project has a very simple code of conduct. Specifically we use the DBAD code of conduct, which includes the following rules:

**1. DON'T BE A DICK **

Seriously. It's pretty simple. Also, who determines if you're being a dick? I do. End of Code of Conduct...

** Contributor License Agreement **

By contributing your code to Craig's Utility Library you grant James Craig a non-exclusive, irrevocable, worldwide, royalty-free, sublicenseable, transferable license under all of Your relevant intellectual property rights (including copyright, patent, and any other rights), to use, copy, prepare derivative works of, distribute and publicly perform and display the Contributions on any licensing terms, including without limitation: (a) open source licenses like the MIT license; and (b) binary, proprietary, or commercial licenses. Except for the licenses granted herein, You reserve all right, title, and interest in and to the Contribution.

You confirm that you are able to grant us these rights. You represent that You are legally entitled to grant the above license. If Your employer has rights to intellectual property that You create, You represent that You have received permission to make the Contributions on behalf of that employer, or that Your employer has waived such rights for the Contributions.

You represent that the Contributions are Your original works of authorship, and to Your knowledge, no other person claims, or has the right to claim, any right in any invention or patent related to the Contributions. You also represent that You are not legally obligated, whether by entering into an agreement or otherwise, in any way that conflicts with the terms of this license.

James Craig acknowledges that, except as explicitly described in this Agreement, any Contribution which you provide is on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING, WITHOUT LIMITATION, ANY WARRANTIES OR CONDITIONS OF TITLE, NON-INFRINGEMENT, MERCHANTABILITY, OR FITNESS FOR A PARTICULAR PURPOSE.