
# Xperience Community AIUN Conversational AI Bot

[![CI: Build and Test](https://github.com/Kentico/repo-template/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/rbt-cms/xperience-community-aiun-conversational-ai-bot/blob/main/.github/workflows/ci.yml)


## Description

This integration enables you to add an AI-powered chatbot to the Xperience by Kentico website using a custom module called AIUN Conversational AI bot (_commonly known as the AIUN Chatbot_). The chatbot intelligently indexes the content of all your website pages and enables visitors to ask questions in natural, conversational language.

Powered by AIUN’s advanced NLP technology, the chatbot understands human language, extracts key information from the page content, and delivers smart, context-aware responses. This provides a more interactive and intelligent experience, replacing traditional keyword-based search with meaningful conversation.

**Core Features:**
1. Conversational AI Integration:
   - Integrates an AI-powered chatbot (AIUN Chatbot) into Xperience by Kentico websites
   - Replaces traditional keyword-based search with smart, conversational interactions
2. Natural Language Processing (NLP):
   - Understands user queries using AIUN’s NLP engine
   - Extracts key content information and provides context-aware answers
3. Content Indexing:
   - Indexes all published pages in the Content Tree to allow meaningful conversation
   - One-time indexing; intelligently handles future content additions or updates without requiring reindexing
4. 200K Free Tokens Included:
   - Comes with 200,000 free tokens for initial use of the AIUN platform
   - Helps users get started with no upfront token purchase required

![AIUN Chatbot](/images/Chatbot-in-website.png)

## Requirements

### Library Version Matrix


| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 29.6.0         | 1.0.0           |

### Dependencies


- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.kentico.com)
- [AIUN Platform](https://qa-dashboard.aiun.ai/)

## Package Installation


Add the package to your application using the .NET CLI

```powershell
dotnet add package XperienceCommunity.AIUNConversationalAIBot
```

## Quick Start

## Step-by-Step Installation Guide

### 1. Register chatbot service in your source code
**1.1. Add the following in your Program.cs:**
```
builder.Services.AddXperienceCommunityAIUNChatbot();
```

### 2. Update your view files:
**2.1. In Views/_ViewImports.cshtml, add:**

```
@addTagHelper *, XperienceCommunity.AIUNConversationalAIBot
```

**2.2. In your layout file (e.g., _Layout.cshtml), right before the </body> tag, add**
```
<vc:aiun-chatbot />
```


**2.3. Build your solution and view the module in the CMS Admin**

AIUN Chatbot in Xpereince by Kentico CMS Dashboard:

![XBYK Dashboard](/images/XBYK_Dashboard.png)


## Use DancingGoat sample

You can restore database with configured samples. View [DancingGoat Sample Database]().

## Full Instructions


View the [Usage Guide](./docs/Usage-Guide.md) for more detailed instructions.


## Reporting issues

Please report any issues seen, in the issue list. We will address at the earliest possibility.


## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.


