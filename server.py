from socket import socket, gethostbyname, AF_INET, SOCK_DGRAM, SOCK_STREAM
import sys

PORT_NUMBER = 5000
SIZE = 1024

hostName = gethostbyname('')

mySocket = socket(AF_INET, SOCK_STREAM)
mySocket.bind((hostName, PORT_NUMBER))
print("hostName: " + str(hostName) + "\n");
print("Test server listening on port {0}\n".format(PORT_NUMBER))

flag = "True"

while flag == "True":
    # Show that data was received:
    (data, addr) = mySocket.recv(SIZE)
    print("Received packet from: " + str(addr) + ", X value:" + data.decode())
