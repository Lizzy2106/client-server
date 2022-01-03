# client-server
This app provid a socket configuration for c# socket application
SocketConfig class provid method for client and server communication :
- Connect(Socket remote) and Disconnect(Socket remote)
- AllowConnection(Socket listener) only for server
- SendMessage(Socket remote, object msg) and ReceiveMessage(Socket socket) for binary messages
- Send(Socket remote, object msg) and Receive(Socket socket) for string messages. This involves using a module class (MessageModule) that represents the format of the object to be transferred. And the JsonConvert class to convert the message from string to object
- ByteArrayToObject(byte[] arrBytes) and ObjectToByteArray(object obj) for conversion.
