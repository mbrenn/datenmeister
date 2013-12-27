BurnSystems.dll
===============

This library contains many helper methods which eases the usage of .Net and avoids the importing of bigger libraries like NLog by offering only a small subset of functions.
The library contains the following modules:

 * Collection Helpers
 * Reading of command line
 * Some object extensions to ease transformation to string
 * A logger like NLog
 * A json builder
 * A reader for Multipart Forms according to RFC 2388
 * An activation container for dependency injection
 * A plugin loader by class attribute
 * An abandoned Serialization interface, which is not usable for migration of serialized data from x64 to x86 or vice versa
 * Synchronisation helper for ReadWrite lock and ThreadWatcher which guards the maximum allowed time for a thread
 * Some string, datetime and environment helpers

