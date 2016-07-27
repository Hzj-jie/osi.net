
#include <gcroot.h>
using System::IDisposable;
using osi::root::lock::slimlock::islimlock;

namespace osi { namespace root { namespace lock { namespace _internal
{
    template <typename T>
    class native_reference
    {
    public:
        gcroot<T^> ref;
        native_reference(T^% i) : ref(i) { }
    };

    public ref struct autolock : public IDisposable
    {
    private:
        native_reference<islimlock>* ref;
    public:
        autolock(islimlock^% i) : ref(new native_reference<islimlock>(i))
        {
            ref->ref->wait();
        }

        !autolock()
        {
            if (ref != nullptr)
            {
                ref->ref->release();
                delete ref;
            }
        }

        ~autolock()
        {
            this->!autolock();
        }
    };

#if 0
    generic <typename T>
    public ref struct autolock : public IDisposable
    {
    private:
        _autolock<T> l;
    public:
        autolock(T^% i) : l(i)
        {
            l.wait();
        }

        !autolock()
        {
            l.release();
        }

        ~autolock()
        {
            this->!autolock();
        }
    };
#endif
} } } }

