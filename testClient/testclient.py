import socket

HOST= '192.168.0.21'
PORT= 5411

s = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
s.connect((HOST,PORT))

while True:
	s.recv(4)
	s.sendall(bytes('ab','UTF-8'))