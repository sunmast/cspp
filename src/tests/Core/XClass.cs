using System;

namespace HappyCspp.Tests.Core
{
    public abstract class XClassL0
    {
        public XClassL0()
        {
        }

        public virtual string VirtualProperty
        {
            get
            {
                return Test.ImCalled("XClassL0.VirtualProperty");
            }
        }

        public virtual void VirtualFoo()
        {
            Test.ImCalled("XClassL0.VirtualFoo");
        }

        public abstract void AbstractFoo();
    }

    public class XClassL1 : XClassL0
    {
        public XClassL1()
        {
        }

        public override string VirtualProperty
        {
            get
            {
                return Test.ImCalled("XClassL1.VirtualProperty");
            }
        }

        public override void VirtualFoo()
        {
            Test.ImCalled("XClassL1.VirtualFoo");
        }

        public override void AbstractFoo()
        {
            Test.ImCalled("XClassL1.AbstractFoo");
        }
    }

    public class XClassL2 : XClassL1
    {
        public XClassL2()
        {
        }

        public override string VirtualProperty
        {
            get
            {
                return Test.ImCalled("XClassL2.VirtualProperty");
            }
        }

        public override void VirtualFoo()
        {
            Test.ImCalled("XClassL2.VirtualFoo");
        }

        public override void AbstractFoo()
        {
            Test.ImCalled("XClassL2.AbstractFoo");
        }
    }
}

