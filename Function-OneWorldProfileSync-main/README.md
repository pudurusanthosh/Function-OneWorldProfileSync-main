# Introduction 
This project contains the Azure function to read member detail information from as.sb.lcc.memberprofileupdate.queue Enterprise service bus and syncs data to Amadeus. 
Once the profile is sync process is successful the message is publihsed to as.sb.lcc.amadeuscount.queue  Enterprise service bus

# Oneworldprofilesync
Entire documention of Amadeus profile sync process is available here:  https://itsals.visualstudio.com/E_Retain_MileagePlan/_wiki/wikis/Mileage%20Plan/3359/Amadeus-Integration-for-oneworld
A occurance of message on as.sb.lcc.memberprofileupdate.queue published by Function-Mileageplanprofilepublisher function app triggers this function. The member profile information such as FirstName, Lastname
Member Number and Loyalty Description and Loyalty ID is created /Updated or Deleted based on the changes made in Siebel application.

Only "Individual" member profile are eligible to sent to Amadeus.
