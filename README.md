# C# Simple CRUD User Access Management (UAM)

This is a simple CRUD WebAPI that implement Middleware & JWT Authentication as part of Coursera Project

  

## Getting Started

To get started with these projects, follow these steps:

  

- Clone the repository to your local machine

- Navigate to the project directory

- Compile and run the project ```dotnet watch run```

- Follow the instructions in the project description to complete the project.

  

## Projects

1. Login to get token for authentication later ```api/auth/login```. Credentials are still hardcoded use the information as follow

	```
	{
		"user":"admin",
		"pass":"password"
	}
	```
2.  (Optional) Check whether the token from login value is valid ```api/auth/check-token``` 
	```
	{
		"token":""
	}
	```
3.  List of endpoint for user access : 
	- GET 
	```api/user``` List of users
	```api/user/{id}``` Specific ID
	- POST
	```api/user```
	- PUT
	```api/user/{id}```
	- DELETE
	```api/user/{id}```
	
	JSON Body
	
			{	
				"Username":"",
				"Email":"",
				"Role":"",
			}
