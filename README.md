# ApiChat.Service

1 - Restaurar el backup de la base de datos o ejecutar el script de nombre ChatBot.back o ScriptChatbot.
2 - Entrar a modificar la cadena de conexion que se encuentra en el program del microservicio de nombre ApiChat.Service.
3 - Despues de estos pasos se puede ejecutar el proyecto, el cual consta de 2 microservicios, el primero (Session.Service) es donde el usuario solicita token si se encuentra 
registrado en la base de datos, el segundo microservicio (ApiChat.Service) es el crud el usuario para que etse funciones tienen que ingresar el token generado en session en el boton de authorizacion del swagger.
4 - en el micro servicio de ApiChat.Service esta implementado el hub del signalr para el chat en tiempo real.
