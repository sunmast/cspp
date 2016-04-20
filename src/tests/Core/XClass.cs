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
                return Assert.ImCalled("XClassL0.VirtualProperty");
            }
        }

        public virtual void VirtualFoo()
        {
            Assert.ImCalled("XClassL0.VirtualFoo");
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
                return Assert.ImCalled("XClassL1.VirtualProperty");
            }
        }

        public override void VirtualFoo()
        {
            Assert.ImCalled("XClassL1.VirtualFoo");
        }

        public override void AbstractFoo()
        {
            Assert.ImCalled("XClassL1.AbstractFoo");
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
                return Assert.ImCalled("XClassL2.VirtualProperty");
            }
        }

        public override void VirtualFoo()
        {
            Assert.ImCalled("XClassL2.VirtualFoo");
        }

        public override void AbstractFoo()
        {
            Assert.ImCalled("XClassL2.AbstractFoo");
        }
    }
}

