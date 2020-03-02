# Solution HorizonOfStars
Project SOA Horizon Of Stars

The solution was built one layer abstraction on Azure Server with architecture SOA with specialized microservices and their respective endpoints providing interoperability for client devices.

For support there is also a video with instructions for use and the installer of the mobile application (android) and source code as well as Swagger documentation:

# Presentation
Pdf: https://tinyurl.com/uzr4cv4

Ppt: https://tinyurl.com/yx6avdq7

# Mobile Aplication (.NET C# Unity)
•	APK (Android): https://drive.google.com/file/d/1S4UTIqj1IG8HvgmodU8jv2hmC-ZczrU6/view?usp=sharing

# Video with instructions
•	Drive: https://drive.google.com/file/d/1C1hGI4rdTq5tZhKWZpl_POJa98g6pW-J/view?usp=sharing

# Documentation
•	Swagger: https://app.swaggerhub.com/apis-docs/willythorpe/HorizonofStars/1.0.0

# Endpoints microservices (API Azure with abstract layer) 
•	Recover all planets using graphql to bring only the necessary data

http://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx?op=GetAllPlanets

•	Recover all starships using graphql to bring only the necessary data

http://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx?op=GetAllStarships

•	Recover calc mglt necessary until the planet based on earth and sun

http://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx?op=GetCalcMathMGLT

•	Recovers ship information through graphql name search

http://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx?op=GetFilterStarship

•	Calculate how many stops will be necessary for the ship to reach a planet

http://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx?op=calcStopResupply

•	Insert information to each consultation of users in the database for future b.i. analysis

http://api-horizonofstars.azurewebsites.net/API-HorizonOfStars.asmx?op=insertBI


# DataBase B.I. (forecast data mining)
FATO table dimensions and future ETL to study responses tendencials users

•	IP: 50.116.87.230

•	DB: osmoc913_horizonofstars

•	User: osmoc913_horizon

•	Password: H0ri0n0fSt@rsP@ssw0rd
