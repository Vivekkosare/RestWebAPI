RestWebAPI that makes a call to mock API.
That mock API can Add, Update, Get and delete data.
RestWebAPI implements Minimal API approach to call the mock api and perform the CRUD operations.
To achieve this, the implementations are:
1. SOLID principles
2. Result pattern to handle the success and failures
3. Early returns in the functions
4. Error handling
5. Validations using Fluent API

   
The CRUD operations can be tested by using the RestWebAPI.http file
Run this command under the project folder: dotnet build
dotnet run
Send request from RestWebAPI.http file

   <img width="1204" alt="image" src="https://github.com/user-attachments/assets/cf489039-723e-45f8-8d05-5100655d5f46" />

