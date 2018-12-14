# RabbitMQ_WebAPI_Demo
Demo for RabbitMQ/MongoDB processing POST request in Web API Controllers.

This visual studio 2017 solution requires two docker containers running in the local machine. Please use following commands on a Docker installed machine:

1. docker run -d --hostname my-rabbit --name rabbit-server -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=admin -p 15672:15672 -p 5672:5672 rabbitmq:3-management

2. docker run -d --name mongo_server -p 27017:27017 mongo

	Run inside container Linux bash:

	# mongo

	Inside Mongo >

		-> use test-db
		-> db.createUser({user: 'testuser', pwd: 'Test1234*', roles: [{role: 'readWrite', db: 'test-db'}]})

After running previous docker commands. Run solution (Start projects: RabbitMQ_WebAPI_Demo.Producer and RabbitMQ_WebAPI_Demo.Consumer). Then send sample POST request (Using Postman) to:

EndPoint: http://localhost:64122/api/Vote (Using json/encoding)
Body(Json):

{
  "VoteId": "78945",
  "DogName": "Pepper",
  "UserId": "456487-123123-123123-456456"
}

You will see the processed message into the consumer (Command line window) application and also in MongoDB.
