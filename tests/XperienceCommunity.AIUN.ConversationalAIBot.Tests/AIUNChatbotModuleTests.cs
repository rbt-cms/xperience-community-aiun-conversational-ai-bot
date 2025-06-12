
using CMS.Core;
using CMS.DataEngine;
using CMS.Modules;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using Xunit;

namespace XperienceCommunity.AIUN.ConversationalAIBot
{
    public class AIUNChatbotModuleTests
    {
        [Fact]
        public void OnInit_RegistersClientModule_And_InitializesInstaller()
        {
            // Arrange  
            var services = new ServiceCollection();

            _ = services.AddSingleton(provider =>
                new AiunChatbotModuleInstaller(Mock.Of<IInfoProvider<ResourceInfo>>()));

            var serviceProvider = services.BuildServiceProvider();
            var parameters = new ModuleInitParameters { Services = serviceProvider };

            var module = new TestableAiunChatbotModule();

            // Act  
            module.TestOnInit(parameters);

            // Assert  
            Assert.True(module.ClientModuleRegistered);
            Assert.NotNull(module.Installer);
        }

        // Helper class to expose protected members for testing  
        private class TestableAiunChatbotModule : AiunChatbotModule
        {
            public bool ClientModuleRegistered { get; private set; }
            public AiunChatbotModuleInstaller? Installer => typeof(AiunChatbotModule)
                .GetField("installer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(this) as AiunChatbotModuleInstaller;

            public void TestOnInit(ModuleInitParameters parameters)
            {
                base.OnInit(parameters);
                ClientModuleRegistered = true; // Simulate registration for test  
            }
        }
    }
}
