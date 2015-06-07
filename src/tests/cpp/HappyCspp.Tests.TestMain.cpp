#include "tests.h"

using namespace std;

namespace HappyCspp { namespace Tests {

    int32_t TestMain::Main(_<sys::array<_<sys::string>>> args) {
        for (int32_t i = 0; i < args->get_Length(); i++) {
            std::printf(args.IndexOf<_<sys::string>>(i) += " ");
        }
        return 0;
    }
}}

int main(int argc, char* argv[]) {
    _<sys::array<_<sys::string>>> args = new_<sys::array<_<sys::string>>>(argc - 1);
    for (int i = 1; i < argc; i++) {
        args.IndexOf<_<sys::string>>(i - 1) = new_<sys::string>(argv[i]);
    }
    return HappyCspp::Tests::TestMain::Main(args);
}
