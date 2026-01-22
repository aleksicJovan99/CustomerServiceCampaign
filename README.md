📞 Customer Service Campaign – Recruitment Project

This repository contains a simulation of a real-life customer reward campaign used by large telecommunication companies.
The goal of the project is to demonstrate the full architecture and implementation flow required to collect daily agent inputs, validate customer reward submissions, and later merge campaign data with an external customer purchase report.

🚀 Overview

A telecom company launched a one-week loyalty campaign, allowing each customer service agent to reward up to five customers per day with special discounts for new purchases.
Since the process was manual, mistakes were possible, and after one month the company receives a .csv report containing customers who actually made a purchase.

The task:
✔ Capture daily agent submissions
✔ Enforce daily reward limits
✔ Provide APIs for CRM integrations
✔ Load and merge monthly CSV report
✔ Expose results through secure, reusable APIs
✔ Simulate realistic architecture and flow

🧱 Architecture Summary

The solution is designed with future extensibility in mind and follows a layered structure.

Components

 ∙ Campaign Service – Stores and validates daily agent submissions

 ∙ Customer Lookup Service – Integrates with external SOAP service
  https://www.crcind.com/csp/samples/SOAP.Demo.cls

 ∙ CSV Import Module – Loads monthly .csv purchase report

 ∙ Result Merging Module – Correlates campaign entries and successful purchases

 ∙ REST API Layer – Secure and reusable endpoints, prepared for CRM integrations

 ∙ Database Layer – Stores campaign data and merged results

🔌 Integrations
 SOAP Customer Service (FindPerson)

 Used to validate customer data and simulate the company’s internal customer lookup logic.

 CSV Report Import

 Processed once per campaign lifecycle, then merged with local data to build a final API-ready dataset.

🛡 API Endpoints

 ∙ A set of REST endpoints is provided for:

 ∙ Submitting daily agent customer rewards

 ∙ Checking customer validity

 ∙ Importing CSV purchase data

 ∙ Fetching merged campaign results

 ∙ All APIs are prepared for secure exposure so they can be easily plugged into various CRM platforms.

📊 Data Flow Summary

 1. Agent submits reward form

 2. System validates customer using SOAP API

 3. Daily limit per agent (max 5) enforced

 4. Data stored in campaign database

 5. After one month: CSV purchase report imported

 6. Merged report generated (rewarded + purchased)

 7. CRMs fetch results via exposed REST API

💾 Tech Stack

 ∙ .NET

 ∙ REST APIs

 ∙ SOAP integration

 ∙ CSV processing

 ∙ Database (MySql)


▶️ How to Run

 1. Clone repository

 2. Configure environment variables

 3. Run migrations / seed (ako postoji)

 6. Start API service

 7. Optional: import CSV purchase file

 8. Use API client (Postman) to interact with endpoints
