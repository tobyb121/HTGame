import socket

bcPORT= 5433


sBroadcast = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sBroadcast.bind(('0.0.0.0',bcPORT))
string,HOST=sBroadcast.recvfrom(64)

print(HOST)






s = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
s.connect((HOST[1],5411))

while True:
	s.recv(4)
	s.sendall(bytes('ab','UTF-8'))
