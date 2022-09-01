using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variance
{

    class Base
    {
        public void F() => Console.WriteLine("TBase.F");
        public virtual void G() => Console.WriteLine("TBase.G");
    }

    class Inherited : Base
    {
        public new void F() => Console.WriteLine("Inherited.F");
        public override void G() => Console.WriteLine("Inherited.G");
    }

    class Inherited2 : Base
    {
        public new void F() => Console.WriteLine("Inherited2.F");
        public override void G() => Console.WriteLine("Inherited2.G");
    }

    class InvariantBox<T>
    {
        T m_t;

        public InvariantBox(T t)
        {
            m_t = t;
        }

        public T Output() { return m_t; }
    }

    interface ICovariant<out T>
    {
        T Output();
    }

    interface IContraVariant<in T>
    {
        void Input(T t);
    }

    class VariantBox<T> : ICovariant<T>, IContraVariant<T>
    {
        T m_t;
        public T Output() { return m_t; }
        public void Input(T t) { m_t = t; }
    }

    internal class Program
    {
        static void Hidden()
        {
            Base b = new Base();
            Inherited i = new Inherited();

            b.F();
            i.F();
            b = i;
            b.F();

        }
        static void Overridden()
        {
            Base b = new Base();
            Inherited i = new Inherited();

            b.G();
            i.G();
            b = i;
            b.G();
        }

        static void ArrayVariance()
        {
            Inherited[] ia = new Inherited[1];
            ia[0] = new Inherited();
            Base [] ba = ia;  //dangerous covariance
            ia[0].F();
            ba[0].F();
            ia[0].G();
            ba[0].G();


            //ba[0] = new Inherited2();   // Unhandled Exception: System.ArrayTypeMismatchException: Attempted to access an element as a type incompatible with the array.
        }

        static void TypeInVariance() 
        {
            InvariantBox<Inherited> box = new InvariantBox<Inherited>(new Inherited());
            box.Output().F();
            box.Output().G();

            //InvariantBox<Base> bbox = box;  //there is no class covariance
        }

        static void InterfaceVariance()
        {
            VariantBox<Base> vb_base = new VariantBox<Base>();
            VariantBox<Inherited> vb_inherited = new VariantBox<Inherited>();

            vb_inherited.Input(new Inherited());
            Inherited i = vb_inherited.Output();

            ICovariant<Base> covariant = vb_inherited; //covariant substitution
            IContraVariant<Inherited> contra = vb_base; //contravariant substitution

            Base bo = covariant.Output();  //typesafe 
            contra.Input(i); //typesafe

            //covariant.Input(i); // no input => safe  can't input Interface2 to Interface box, through base box
            //contra.Output(); // no Output => safe  otherwise Output would be Inherited type, runtime type would be only base, => runtime error
        }
        static void Main()
        {
            Action newLine = Console.WriteLine;

            Hidden();
            newLine();
            Overridden();
            newLine();
            ArrayVariance();
            newLine();
            TypeInVariance();
            newLine();
            InterfaceVariance();
        } 
    }
}
