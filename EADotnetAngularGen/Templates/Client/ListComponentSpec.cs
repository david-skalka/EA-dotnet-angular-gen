﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace EADotnetAngularGen.Templates.Client
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using CaseExtensions;
    using EA;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class ListComponentSpec : ListComponentSpecBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("import { ComponentFixture, TestBed } from '@angular/core/testing';\r\nimport { BrowserAnimationsModule } from '@angular/platform-browser/animations';\r\nimport { of } from 'rxjs';\r\nimport { MatDialog } from '@angular/material/dialog';\r\nimport { ");
            
            #line 12 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ListComponent } from './");
            
            #line 12 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("-list.component';\r\nimport { ");
            
            #line 13 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write(", ");
            
            #line 13 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("Service, ");
            
            #line 13 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ServiceInterface } from '../api';\r\n\r\ndescribe('");
            
            #line 15 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ListComponent', () => {\r\n  let component: ");
            
            #line 16 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ListComponent;\r\n  let fixture: ComponentFixture<");
            
            #line 17 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ListComponent>;\r\n\r\n  let ");
            
            #line 19 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("ServiceSpy: jasmine.SpyObj<");
            
            #line 19 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ServiceInterface> | null = null;\r\n\r\n  const ");
            
            #line 21 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase().Pluralize()));
            
            #line default
            #line hidden
            this.Write(" = [");
            
            #line 21 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.JsObjectInitializer( Model.Attributes.Cast<EA.Attribute>().Where(x=>x.IsTypePrimitive()).ToDictionary(x=>x.Name, x=>x.GetFakeValue()) )));
            
            #line default
            #line hidden
            this.Write(" as ");
            
            #line 21 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("];\r\n\r\n  \r\n  const matDialogSpy = jasmine.createSpyObj<MatDialog>(['open']);\r\n  matDialogSpy.open.and.returnValue({afterClosed : jasmine.createSpy('afterClosed').and.returnValue(of(null)) as unknown} as never);\r\n\r\n  beforeEach(async () => {\r\n\r\n    ");
            
            #line 29 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("ServiceSpy = jasmine.createSpyObj<");
            
            #line 29 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ServiceInterface>(['");
            
            #line 29 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Get']);\r\n\r\n    ");
            
            #line 31 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("ServiceSpy.");
            
            #line 31 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Get.and.returnValue(of(");
            
            #line 31 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase().Pluralize()));
            
            #line default
            #line hidden
            this.Write(")  );\r\n\r\n    await TestBed.configureTestingModule({\r\n      imports: [BrowserAnimationsModule],\r\n      providers: [{ provide: ");
            
            #line 35 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("Service, useValue: ");
            
            #line 35 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("ServiceSpy }, {provide: MatDialog, useValue: matDialogSpy}],\r\n      declarations: [\r\n        \r\n      ]\r\n    })\r\n    .compileComponents();\r\n\r\n    fixture = TestBed.createComponent(");
            
            #line 42 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("ListComponent);\r\n    component = fixture.componentInstance;\r\n\r\n\r\n\r\n  });\r\n\r\n  it('should ngOnInit', async () => {\r\n    await component.ngOnInit();\r\n    expect(component.dataSource.data).toEqual(");
            
            #line 51 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase().Pluralize()));
            
            #line default
            #line hidden
            this.Write(");\r\n  });\r\n\r\n\r\n\r\n  it('should edit', async () => {\r\n    await component.edit(");
            
            #line 57 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase().Pluralize()));
            
            #line default
            #line hidden
            this.Write("[0].id);\r\n    expect(component.dataSource.data).toEqual(");
            
            #line 58 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase().Pluralize()));
            
            #line default
            #line hidden
            this.Write(");\r\n  });\r\n\r\n \r\n  it('should add', async () => {\r\n    await component.add();\r\n    expect(component.dataSource.data).toEqual(");
            
            #line 64 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.ToCamelCase().Pluralize()));
            
            #line default
            #line hidden
            this.Write(");\r\n  });\r\n\r\n\r\n\r\n\r\n \r\n\r\n\r\n});\r\n\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 76 "C:\Users\David\source\repos\EA-dotnet-angular-gen\EADotnetAngularGen\Templates\Client\ListComponentSpec.tt"


public EA.Element Model { get; set; }


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class ListComponentSpecBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
