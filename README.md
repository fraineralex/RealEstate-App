# Real Estate App

Real estate application that manages different roles each with different functionalities and interactions in the system, with an integrated restful API to consume all the functionalities. It is developed under the ONION architecture in C# ASP .Net Core following the SOLID principles.

##### Test the demo [HERE]( "HERE")
<br><br>
## Technologies 

- Frontend
	- html
  - css
    - Bootstrap
  - js
  - ASP.NET Razor


- Backend
	- C# ASP.NET Core (6.0)
      - Microsoft Entity Framework Core
      - Microsoft Entity Framework Core Relational
      - Microsoft Entity Framework Core SqlServer
      - Microsoft Entity Framework Core Tools
      - Microsoft Entity Framework Core Design
      - Microsoft Entity Framework Code First
      - Microsoft.AspNetCore.Mvc.NewtonsoftJson
      - Microsoft.AspNetCore.Mvc.Versioning
      - Microsoft.VisualStudio.Web.CodeGeneration.Design
      - Swashbuckle.AspNetCore.Swagger
      - Swashbuckle.AspNetCore.Annotations
      - Swashbuckle.AspNetCore
      - Microsoft.AspNetCore.Authentication.JwtBearer
      - Microsoft.AspNetCore.Identity.EntityFrameworkCore
      - CQRS Pattern
      - Mediator Pattern

- ORM
  - Entity Framework
  
- DB
  - SQL Server
  
 <br><br>
 ## Project images
 
 - Login 
 
[![login.png](https://github.com/fraineralex/RealEstate-App/blob/master/RealEstateApp.Presentation.WebApp/wwwroot/Images/login.png)](https://github.com/fraineralex/RealEstate-App/blob/master/RealEstateApp.Presentation.WebApp/wwwroot/Images/login.png)
<br>
 - Dashboard 
 
[![dashboard.png](https://github.com/fraineralex/RealEstate-App/blob/master/RealEstateApp.Presentation.WebApp/wwwroot/Images/dashboard.png)](https://github.com/fraineralex/RealEstate-App/blob/master/RealEstateApp.Presentation.WebApp/wwwroot/Images/dashboard.png)
<br>
 
<br><br>
## What do you need to run this project ?

- Visual Studio 2022 onwards
- ASP.NET - v6.0 onwards
- SQL Server - v3.39.2 onwards

<br><br>
## Installation

- Download the project or clone it
   - download [CLICK AQUI](https://github.com/fraineralex/RealEstate-App/archive/refs/heads/master.zip)
   - clone [CLICK AQUI](https://github.com/fraineralex/RealEstate-App.git)

- You need to open the project where is located using Visual Studio 2022.
```js
 //C:\Users\Frainer Alexander\Desktop\RealEstate-App>  - take this path whit example
```

- Now you need to open the file called "appsettings.json" and put the name of your server or computer where it belongs, example:
```cmd
Server=Lenovo-8517;
```

- Then in Visual Studio go to:
```cmd
Tools/NuGet packages manager/Package management console
```

- When you are in the console type the following command:
```cmd
Update-Database -Context ApplicaionContext
```
and
```cmd
Update-Database -Context IdentityContext
```

- Now run the project and the application will run in your default browser. 

<br><br>
## Developers
- Frainer Encarnación -> [Github](https://github.com/fraineralex) 
- Ronal Cadiz -> [Github](https://github.com/Ronaldcdz) 
- Cristopher Zaiz -> [Github](https://github.com/zaizo01) 





