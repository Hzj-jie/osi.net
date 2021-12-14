﻿
using System.Collections.Generic;
using System.IO;
using System;

public static class Program {

  private static void w(params string[] s) {
    if (s == null) return;
    for (int i = 0; i < s.Length; i++) {
      Console.Write(s[i]);
    }
  }

  private static void wl(params string[] s) {
    w(s);
    Console.WriteLine();
  }

  public static void Main() {
    wl();
    wl("\' This file is generated by builder_gen and definition.txt.");
    wl("\' Do not edit.");
    wl();
    wl("Option Explicit On");
    wl("Option Infer Off");
    wl("Option Strict On");
    wl();
    wl("Imports osi.root.connector");
    wl("Imports osi.root.constants");
    wl("Imports osi.root.formation");
    wl("Imports osi.service.interpreter.primitive");
    wl();
    wl("Namespace logic");
    wl("    Partial Public NotInheritable Class builders");
    int keys_count = 0;
    string s;
    while ((s = Console.ReadLine()) != null) {
      s = s.Trim();
      if (string.IsNullOrWhiteSpace(s) || s[0] == '#') {
        continue;
      }
      string[] ss = s.Split('\t');
      keys_count++;
      List<string> parameters = new List<string>();
      List<string> parameter_types = new List<string>();
      List<string> passed_parameters = new List<string>();
      List<string> append_parameters = new List<string>();
      for (int i = 1; i < ss.Length; i++) {
        string parameter;
        switch(ss[i]) {
          case "string":
            parameter = "String";
            break;
          case "uint":
            parameter = "UInt32";
            break;
          case "data_block":
            parameter = "data_block";
            break;
          case "parameters":
            parameter = "vector(Of String)";
            break;
          case "typed_parameters":
            parameter = "vector(Of pair(Of String, String))";
            break;
          case "paragraph":
            parameter = "Func(Of writer, Boolean)";
            break;
          default:
            parameter = null;
            break;
        }
        if (parameter != null) {
          parameters.Add("ByVal " + ss[i] + "_" + i.ToString() + " As " + parameter);
          parameter_types.Add(parameter);
          passed_parameters.Add(ss[i] + "_" + i.ToString());
          if (ss[i] == "paragraph") {
            append_parameters.Add("\"{\"");
          } else if (ss[i] == "parameters" || ss[i] == "typed_parameters") {
            append_parameters.Add("\"(\"");
          }
          append_parameters.Add(ss[i] + "_" + i.ToString());
          if (ss[i] == "paragraph") {
            append_parameters.Add("\"}\"");
          } else if (ss[i] == "parameters" || ss[i] == "typed_parameters") {
            append_parameters.Add("\")\"");
          }
        } else {
          append_parameters.Add("\"" + ss[i] + "\"");
        }
      }
      wl();
      wl("        Public Shared Function of_" + ss[0] + "(" + string.Join(", ", parameters) + ") As " +
             ss[0] + "_builder_" + keys_count.ToString());
      wl("            Return New " + ss[0] + "_builder_" + keys_count.ToString() +
                        "(" + string.Join(", ", passed_parameters) + ")");
      wl("        End Function");
      wl();
      wl("        Public NotInheritable Class " + ss[0] + "_builder_" + keys_count.ToString());
      wl();
      for (int i = 0; i < parameters.Count; i++) {
        wl("            Private ReadOnly " + passed_parameters[i] + " As " + parameter_types[i]);
      }
      if (parameters.Count > 0) {
        wl();
        wl("            Public Sub New(" + string.Join(", ", parameters) + ")");
        for (int i = 0; i < parameters.Count; i++) {
          if (parameter_types[i] == "String") {
            wl("                assert(Not " + passed_parameters[i] + ".null_or_whitespace())");
          } else if (parameter_types[i] != "UInt32") {
            wl("                assert(Not " + passed_parameters[i] + " Is Nothing)");
          }
          wl("                Me." + passed_parameters[i] + " = " + passed_parameters[i]);
        }
        wl("            End Sub");
        wl();
      }
      wl("            Public Function [to](ByVal o As writer) As Boolean");
      wl("                Return _");
      wl("                    o.append(\"" + ss[0] + "\") AndAlso");
      for (int i = 0; i < append_parameters.Count; i++) {
        wl("                    o.append(" + append_parameters[i] + ") AndAlso");
      }
      wl("                    o.append(newline.incode())");
      wl("            End Function");
      wl("        End Class");
    }
    wl("    End Class");
    wl("End Namespace");
  }
}
