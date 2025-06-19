# Step-by-Step Usage Guide

**1. How to Register with AIUN from XBYK CMS Admin**
You can create your AIUN account easily using the "Register with AIUN" form on the Module. Follow these steps:
 ---Screenshot for CMS Admin Dashboard

Step 1: Access the Registration with AIUN Form
 ---Screenshot
Go to the "Register with AIUN" page.

If you are logged in as an Administrator, the form will be automatically prefilled with your:

First Name

Last Name

Email Address

You can edit these fields if needed


Step 2: Submit Registration Details
Click Register after filling out or confirming your details

The system will:

Send your information to the AIUN platform

Display a message that registration is in progress

Step 3: Verify Your Email
Check your email inbox for a message from the AIUN platform

Click the verification link in the email to verify your email address

Step 4: Receive Login Credentials & API Key
After verifying your email, you will receive:

Username and Password to log in to the AIUN platform

API Key used to connect Kentico with the AIUN platform

These will be sent via email.

Step 5: Enter Your API Key in the Form
Return to the "Register with AIUN" page

The form will change and display an input field for the API Key

Paste your API Key into this field

Step 6: Save API Key
The Register button will now change to Save

Click Save to complete the setup

Your CMS is now securely connected to the AIUN platform!

**Login to AIUN Platform to setup Chatbot**
2.1. Login:
--Screenshot
Need help? Contact us at support@aiun.ai

2.2. Configure Your Chatbot in AIUN Dashboard
Log in to your AIUN account
Go to Chatbot link

---Screenshot
Click Add Chat Widget button then fill:
---Screenshot

Chat Bot Name: e.g. AI Assistant
Foreground: e.g. #FFFFF
Background Color: e.g. #000000
Allowed Website URL: e.g. Enter the website URL where you want the chat widget to appear
Status: The default status is Active. Deactivate it if you don't want the chat widget to appear on your website
Welcome Message: Enter welcome message
After saving, the chatbot widget script will be generated – this is required for integration with Kentico

---Screenshot

4. Link AIUN Chatbot with Kentico
1. From AIUN Dashboard:
1.1. Copy your API Key from the Profile page Or You can copy API key from email which was received from AIUN

---Screenshot

1.2. Get the Client ID, API Key, and Base URL from the chatbot widget script

--- Screenshot

In Kentico Admin:
2.1. Go to AIUN Chatbot --> Register with AIUN> API Key → Paste the API Key (Refer to section 1.1 above for value references) and Save

---Screenshot

2.2. Go to Configurations > 2.2 Go to AIUN Chatbot --> Configuration

---Screenshot

2.3 Click Create Button to configure Chat bot. Select Channel → Paste the Client ID, API Key, Base URL (Refer to section 1.2 above for value references)

---Screenshot

2.4 Save to view the list of configurations

----Screenshot

5. Index Your Website Content
Go to the Configurations Page in Kentico
Use the “Index Published Pages” button to send your site’s content to the AIUN platform -To index all published pages in the Content Tree, push them from Kentico to the AIUN Platform by clicking the 'Index Published Pages' action button in the list.

---Screenshot

6. Track progress under:
Go to Indexes Page – to check status

---Screenshot

Go to Token Usage Page – to view usage stats

-- Screenshot












