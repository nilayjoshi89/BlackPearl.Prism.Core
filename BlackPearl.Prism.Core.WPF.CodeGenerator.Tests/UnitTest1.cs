using System;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackPearl.Prism.Core.WPF.CodeGenerator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string inputCode = File.ReadAllText("MyTestViewModel.cs");
            Compilation inputCompilation = CreateCompilation(inputCode, GetMetaReferences());

            var generator = new ViewModelGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out Compilation outputCompilation, out System.Collections.Immutable.ImmutableArray<Diagnostic> diagnostics);
            GeneratorDriverRunResult runResult = driver.GetRunResult();
            GeneratorRunResult generatorResult = runResult.Results[0];
            foreach (GeneratedSourceResult gs in generatorResult.GeneratedSources)
            {
                File.WriteAllText("..\\..\\..\\" + gs.HintName, gs.SourceText.ToString());
            }
        }

        protected static Compilation CreateCompilation(string source, MetadataReference[] metadataReferences)
                => CSharpCompilation.Create("compilation",
                    new[] { CSharpSyntaxTree.ParseText(source) },
                    metadataReferences,
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        private static PortableExecutableReference[] GetMetaReferences() =>
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToArray();
    }
}
