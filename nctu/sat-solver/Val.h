#pragma once
#include <exception>
using namespace std;
enum BOOL_VAL { F, T, N };
BOOL_VAL NOT(BOOL_VAL v) {
        if (v == N)
                    return N;
            else
                        return (v == T) ? F : T;
}
typedef int LEVEL_VAL; typedef unsigned int VAR_INDEX; typedef unsigned int COUNT_VAL;

struct RealUNSAT : public exception {};
struct JustBackTrack : public exception {};
const JustBackTrack BT;
const RealUNSAT GG;