# RabbitMQ_WebAPI_Demo
Demo for RabbitMQ processing POST request in Web API Controllers.

This visual studio 2017 solution requires a docker contanier running in the local machine. Please use following command on a Docker installed machine:

docker run -d --hostname my-rabbit --name rabbit-server -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=admin -p 15672:15672 -p 5672:5672 rabbitmq:3-management

After running previous docker command. Run solution (Start projects: RabbitMQ_WebAPI_Demo.Producer and RabbitMQ_WebAPI_Demo.Consumer). Then send sample POST request (Using Postman) to:

EndPoint: http://localhost:64122/api/Vote (Using json/encoding)
Body(Json):

{
  "VoteId": "78945",
  "DogName": "Pepper",
  "UserId": "456487-123123-123123-456456"
}

You will see the processed message into the consumer (Command line window) application.
