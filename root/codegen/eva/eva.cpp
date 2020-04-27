
#include <stdio.h>
#include <string.h>
#include <string>
using namespace std;

const char* types[] = {"Decimal",
                       "Int64",
                       "Int32",
                       "Int16",
                       "SByte",
                       "UInt64",
                       "UInt32",
                       "UInt16",
                       "Byte",
                       "Single",
                       "Double",
                       "String",
                       "Boolean"};
const char* pointer_types[] = {"pointer",
                               "array_pointer",
                               "weak_pointer"};

void eva1(FILE* fo)
{
    fputs("        x = y\n"
          "        Return True\n"
          "    End Function\n\n",
          fo);
}

void eva2(FILE* fo)
{
    fputs("        If y Is Nothing Then\n"
          "            Return False\n"
          "        Else\n"
          "            x = y()\n"
          "            Return True\n"
          "        End If\n"
          "    End Function\n\n",
          fo);
}

void eva3(FILE* fo)
{
    fputs("        If Not x Is Nothing Then\n"
          "            x.set(y)\n"
          "        End If\n"
          "        Return True\n"
          "    End Function\n\n",
          fo);
}

void eva4(FILE* fo)
{
    fputs("        If y Is Nothing Then\n"
          "            Return False\n"
          "        Else\n"
          "            If Not x Is Nothing Then\n"
          "                x.set(y())\n"
          "            End If\n"
          "            Return True\n"
          "        End If\n"
          "    End Function\n\n",
          fo);
}

int main(int argc, char* argv[])
{
    if(argc < 2) printf("need a parameter for output file.\n");
    else
    {
        FILE* fo = fopen(argv[1], "w");
        fputs("\n'this file is generated by osi/root/codegen/eva/eva.exe"
              "\n'so edit the osi/root/codegen/eva/eva.cpp instead of this file\n\n"
              "Option Explicit On\n"
              "Option Infer Off\n"
              "Option Strict On\n\n"
              "Imports System.Runtime.CompileServices\n"
              "Imports osi.root.constants\n"
              "Imports osi.root.formation\n"
              "\nPublic Module _eva\n\n"
              "#If Not DEBUG Then\n\n", fo);
        for(int i = 0; i < sizeof(types) / sizeof(types[0]); i++)
        {
            string t;
            for(int j = 0; j < 2; j++)
            {
                t = types[i];
                if(j == 1) t += "()";
                fprintf(fo,
                        "    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
                        "    Public Function eva(ByRef x As %s, ByVal y As %s) As Boolean\n",
                        t.c_str(), t.c_str());
                eva1(fo);
                /*
                fprintf(fo,
                        "    Public Function eva(ByRef x As %s, ByVal y As _do(Of %s)) As Boolean\n",
                        t.c_str(), t.c_str());
                eva2(fo);
                */
                fprintf(fo,
                        "    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
                        "    Public Function eva(ByRef x As %s, ByVal y As Func(Of %s)) As Boolean\n",
                        t.c_str(), t.c_str());
                eva2(fo);
                for(int k = 0; k < sizeof(pointer_types) / sizeof(pointer_types[0]); k++)
                {
                    string ext;
                    if(string(pointer_types[k]) == "array_pointer") ext = "()";
                    fprintf(fo,
                            "    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
                            "    Public Function eva(ByVal x As %s(Of %s), ByVal y As %s%s) As Boolean\n",
                            pointer_types[k], t.c_str(), t.c_str(), ext.c_str());
                    eva3(fo);
                    /*
                    fprintf(fo,
                            "    Public Function eva(ByVal x As %s(Of %s), ByVal y As _do(Of %s%s)) As Boolean\n",
                            pointer_types[k], t.c_str(), t.c_str(), ext.c_str());
                    eva4(fo);
                    */
                    fprintf(fo,
                            "    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
                            "    Public Function eva(ByVal x As %s(Of %s), ByVal y As Func(Of %s%s)) As Boolean\n",
                            pointer_types[k], t.c_str(), t.c_str(), ext.c_str());
                    eva4(fo);
                }
            }
        }

        fputs("#End If\n\n", fo);
        fputs("    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
              "    Public Function eva(Of T)(ByRef x As T, ByVal y As T) As Boolean\n", fo);
        eva1(fo);
        /*
        fputs("    Public Function eva(Of T)(ByRef x As T, ByVal y As _do(Of T)) As Boolean\n", fo);
        eva2(fo);
        */
        fputs("    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
              "    Public Function eva(Of T)(ByRef x As T, ByVal y As Func(Of T)) As Boolean\n", fo);
        eva2(fo);
        for(int i = 0; i < sizeof(pointer_types) / sizeof(pointer_types[0]); i++)
        {
            string ext;
            if(string(pointer_types[i]) == "array_pointer") ext = "()";
            fprintf(fo,
                    "    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
                    "    Public Function eva(Of T)(ByVal x As %s(Of T), ByVal y As T%s) As Boolean\n",
                    pointer_types[i], ext.c_str());
            eva3(fo);
            /*
            fprintf(fo,
                    "    Public Function eva(Of T)(ByVal x As %s(Of T), ByVal y As _do(Of T%s)) As Boolean\n",
                    pointer_types[i], ext.c_str());
            eva4(fo);
            */
            fprintf(fo,
                    "    <MethodImpl(method_impl_options.aggressive_inlining)>\n"
                    "    Public Function eva(Of T)(ByVal x As %s(Of T), ByVal y As Func(Of T%s)) As Boolean\n",
                    pointer_types[i], ext.c_str());
            eva4(fo);
        }

        fputs("End Module\n", fo);
        fclose(fo);
    }
}

