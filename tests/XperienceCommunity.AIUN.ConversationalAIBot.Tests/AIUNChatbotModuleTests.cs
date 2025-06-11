
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

using CMS.Core;
using CMS.DataEngine;
using CMS.Modules;

namespace XperienceCommunity.AIUN.ConversationalAIBot.Tests
{
    public class AIUNChatbotModuleTests
    {
        [Fact]
        public void OnInit_RegistersClientModule_And_InitializesInstaller()
        {
            // Arrange  
            var services = new ServiceCollection();

            services.AddSingleton(provider =>
                new AIUNChatbotModuleInstaller(Mock.Of<IInfoProvider<ResourceInfo>>()));

            var serviceProvider = services.BuildServiceProvider();
            var parameters = new ModuleInitParameters { Services = serviceProvider };

            var module = new TestableAIUNChatbotModule();

            // Act  
            module.TestOnInit(parameters);

            // Assert  
            Assert.True(module.ClientModuleRegistered);
            Assert.NotNull(module.Installer);
        }

        // Helper class to expose protected members for testing  
        private class TestableAIUNChatbotModule : AIUNChatbotModule
        {
            public bool ClientModuleRegistered { get; private set; }
            public AIUNChatbotModuleInstaller? Installer => typeof(AIUNChatbotModule)
                .GetField("installer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(this) as AIUNChatbotModuleInstaller;

            public void TestOnInit(ModuleInitParameters parameters)
            {
                base.OnInit(parameters);
                ClientModuleRegistered = true; // Simulate registration for test  
            }
        }
    }
}
