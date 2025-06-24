# Step-by-Step Usage Guide

## 1. How to Register with AIUN – AIUN Chatbot Module from XBYK CMS Admin

You can create your AIUN account easily using the "Register with AIUN" Form on the Module. Follow these steps:

**Pre-requisites:**
 - Install AIUN Chatbot Module

**Step 1: Access the Registration with AIUN Form**

1.1. Go to the "Register with AIUN" page - If you are logged in as an Administrator, the form will be automatically prefilled with your:
- First Name
- Last Name
- Email Address
  
**Note:** You can edit these fields if needed

 ![Register with AIUN Form](/images/RegisterwithAIUN-1.png)

**Step 2: Submit Registration Details**

Click **Register** after filling out or confirming your details

The system will:
  - Send your information to the AIUN platform
  - Display a message that registration "Display a message: 'Verification email sent. Please confirm to activate your API key.'"

![Email 1 ](/images/RegisterwithConfirmation.png)

**Step 3: Verify Your Email**

- Check your email inbox for a message from the AIUN platform
- Click the verification link in the email to verify your email address

![Email 1](/images/Email_1.png)

**Step 4: Receive Login Credentials & API Key**

After verifying your email, you will receive:

- Username and Password to log in to the AIUN platform
- API Key used to connect Kentico with the AIUN platform

These will be sent via email.

![Email 2](/images/Email_2.png)



**Step 5: Enter Your API Key in the Form**

Return to the "Register with AIUN" page
- The form will change and display an input field for the API Key
- Paste your API Key into this field

**Step 6: Save API Key**

- The **Register** button will now change to **Save**
- Click **Save** to complete the Registration

Your CMS is now securely connected to the AIUN platform!

## 2. Login to AIUN Platform to setup Chatbot

**Pre-requisites:**
 - Install AIUN Chatbot Module
 - Register with AIUN from Module

**2.1. Login:**

- Log in to your AIUN account
  
![AIUN Platform Login Page](/images/AIUN_Login.png)


If you need help? Please contact us at support@aiun.ai

**2.2. Configure Your Chatbot in AIUN Dashboard**

- Go to Chatbot link

![ChatBot Page](/images/AIUN-Chatbot_List_Empty.png)

- Click Add **Chatbot** button then fill the following:

  ![Chatbot Configuration from AIUN Palatform](/images/AIUN-Chatbot_Form.png)
  
1. Chatbot Name: e.g. AI Assistant
2. Foreground: e.g. #FFFFF
3. Background Color: e.g. #000000
4. Allowed Domains: e.g. Enter the website URL where you want the chat widget to appear
5. Status: The default status is Active. Deactivate it if you don't want the chat widget to appear on your website
6. Welcome Message: Enter welcome message

After saving, the chatbot widget script will be generated – this is required for integration with AIUN Chatbot Module from XBYK and showing list -

![Chatbot List](/images/AIUN-Chatbot_List.png)

You can click on Edit Button and see the ClientID, API Key and generated Script for website.

![Chatbot Configuration Edit View](/images/AIUN-Chatbot_Edit_View.png)

## 3. Link AIUN Chatbot with Kentico

**3.1. From AIUN Dashboard:**

3.1.1. Copy your API Key from email which was received from AIUN Or You can copy API key from the Profile page

![AIUN Profile Page](/images/AIUN_Profile_Page.png)

3.1.2. Copy the Client ID, API Key, and Base URL from the chatbot widget Configuration Page (Edit Mode)

![View AIUN Chatbot Widget Configuration Page](/images/AIUN-Chatbot_Edit_View.png)

**3.2. In AIUN Chatbot → XBYK CMS Admin:**

3.2.1. Go to AIUN Chatbot Module → Register with AIUN> API Key → Paste the API Key (Refer to section 3.1.1 above for value references) and Save

![XBKYK AIUN Chatbot Register API Key](/images/RegisterwithAIUN-2.png)

3.2.2. Go to AIUN Chatbot Module → Configuration

![XBYK AIUN Chatbot Configuration Page](/images/XBYK_Module_Chatbot_Configuration.png)

3.2.2.1. Click **Create** Button to configure Chat bot. Select Channel → Paste the Client ID, API Key, Base URL (Refer to section 3.1.2 above for value references)

![XBYK AIUN Chatbot create bot](/images/XBYK_Module_Chatbot_Configuration-Save.png)

3.2.2.2. **Save** to view the list of configurations

![XBYL AIUN Chatbot List](/images/XBYK_Module_ChatbotList.png)

**3.3 Index Your Website Content**

3.3.1. Go to the Configurations Page in Module
3.3.2. Use the “Index Published Pages” button to send your site’s content to the AIUN platform -To index all published pages in the Content Tree, push them from Kentico to the AIUN Platform by clicking the 'Index Published Pages' action button in the list.

![XBYK AIUN Index Published Pages](/images/XBYK_Module_Chatbot_Configuration-List-Index.png)

**3.4. Track Indexes progress under Indexes Page:**

3.4.1. Go to **Indexes** Page – to check status

![XBYK AIUN Indexes state](/images/XBYK_Module_Chatbot_IndexList.png)

**3.5. Check Token Usage**

3.5.1. Go to **Token Usage** Page – to view usage stats

![XBYK AIUN Chatbot Token Usage](/images/TokenUsage.png)












