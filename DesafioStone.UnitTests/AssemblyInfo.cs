using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 100, Scope = ExecutionScope.MethodLevel)]