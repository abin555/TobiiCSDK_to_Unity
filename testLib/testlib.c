#include <stdio.h>

struct TestStruct {
	int a;
	int b;
	float c;
};

int testFunc(){
	printf("HELLO THERE!\n");
	return 42;
}

struct TestStruct testStructFunc(int a, int b, float c){
	struct TestStruct s;
	s.a = a*b;
	s.b = b-a;
	s.c = c * 2;
	return s;
}
