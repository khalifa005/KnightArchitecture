import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormlyFormOptions, FormlyFieldConfig } from '@ngx-formly/core';
import { Subscription, Subject, of } from 'rxjs';
import { convertCommaSeparatedToNumberArray } from '../../@core/utils.ts/data-types.utilts/number-utils';

@Component({
  selector: 'app-formly-form',
  templateUrl: './formly-form.component.html',
  styleUrl: './formly-form.component.scss'
})
export class FormlyFormComponent implements OnInit, OnDestroy {

  private subs: Subscription[] = [];
  private destroy$: Subject<any> = new Subject<any>();

  form: FormGroup;
  isUpdateMode: boolean = false;
  formJsonFileName = "api-dynamic-form-testing.json"
  // formJsonFileName = "api-dynamic-form.json"
  model: any = {};

  options: FormlyFormOptions = {
    formState: {
      selectOptionsData: {}
      // Remove the selectOptionsData property
      //we can set dtopdown item here or get them from api 
    },
  };

  fields: FormlyFieldConfig[];
  type: string;
  formHeader: string = "Dynamic form title";
  firstModelChange: boolean = true;
  showForm: boolean = false;
  formRandomNumber: number;

  examples = [
    // 'simple',
    'json-powered',
    // 'dynamic-form',
    // 'dynamic-form',
    // 'nested',
    // 'arrays',
    // 'numbers',
    // 'references',
    // 'schema_dependencies',
    // 'null_field',
    // 'nullable',
    // 'allOf',
    // 'anyOf',
    // 'oneOf',
    // 'select_alternatives',
  ];

   formDataModel: any = {
    VALVESCLOSING: 1,
    VALVESCOUNT: 1,
    MAINCLASSFICATION: 5001,
    SUBCATEGORY: 5098,
    ACTIONTAKEN: 5099,
    FBFPRIORITY: 12000,
    LOCATIONRELATEDASSET: "11",
    HOUSECONNECTIONNUMBER: "11",
    ATTACHMENT: [
      {
        file: {
          id: 0,
          name: "mahmoud kahlifa.jpg",
          lastModified: 1721433600000,
          lastModifiedDate: "11/25/2024 09:58:40",
          webKitRelativePath: null,
          size: null,
          fbfFieldID: 0,
          physicalPath: null,
        },
        fileID: 7694489,
        filePath: "C:\\20241125\\312773_mahmoud kahlifa.jpg",
      },
      {
        file: {
          id: 0,
          name: "mahmoud kahlifa.jpg",
          lastModified: 1721433600000,
          lastModifiedDate: "12/01/2024 13:10:06",
          webKitRelativePath: null,
          size: null,
          fbfFieldID: 0,
          physicalPath: null,
        },
        fileID: 7694509,
        filePath: "C:\\20241201\\312773_mahmoud kahlifa.jpg",
      },
      {
        file: {
          id: 0,
          name: "logo.png",
          lastModified: 1721433600000,
          lastModifiedDate: "12/01/2024 13:10:56",
          webKitRelativePath: null,
          size: null,
          fbfFieldID: 0,
          physicalPath: null,
        },
        fileID: 7694510,
      },
      {
        file: {
          id: 0,
          name: "logo.png",
          lastModified: 1721433600000,
          lastModifiedDate: "12/01/2024 13:11:31",
          webKitRelativePath: null,
          size: null,
          fbfFieldID: 0,
          physicalPath: null,
        },
        fileID: 7694511,
        filePath: "",
      },
      {
        file: {
          id: 0,
          name: "logo.png",
          lastModified: 1721433600000,
          lastModifiedDate: "12/01/2024 13:12:04",
          webKitRelativePath: null,
          size: null,
          fbfFieldID: 0,
          physicalPath: null,
        },
        fileID: 7694512,
        filePath: "",
      },
    ],
    REMARKS: "11",
    PK: "!!!SPNWCE-1733058728!!!",
    REPORTID: 81,
    FBFId: "5002",
    TaskCode: "312773",
    FormMode: "EDIT",
  };
  
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    // this.loadExample(this.examples[0]);
    this.loadSpecificExample();
  }

  loadSpecificExample() {
    this.showForm = false;
    this.form = new FormGroup({});
    this.options = {};

    this.http
      .get<FormlyFieldConfig[]>(`assets/json-powered/${this.formJsonFileName}`)
      .subscribe(fields => {

        this.model = {
          "MAINCLASSFICATION": 6100,
          "SUBCATEGORY": 6110,
          "ACTIONTAKEN": "6136,6135",
          "LOCATIONRELATEDASSET": "2",
          "HOUSECONNECTIONNUMBER": "23",
          "REMARKS": "some comments",
          ATTACHMENT: [
            {
              file: {
                id: 0,
                name: "mahmoud kahlifa.jpg",
                lastModified: 1721433600000,
                lastModifiedDate: "11/25/2024 09:58:40",
                webKitRelativePath: null,
                size: null,
                fbfFieldID: 0,
                physicalPath: null,
              },
              fileID: 7694489,
              filePath: "",
            },
            {
              file: {
                id: 0,
                name: "mahmoud kahlifa.jpg",
                lastModified: 1721433600000,
                lastModifiedDate: "12/01/2024 13:10:06",
                webKitRelativePath: null,
                size: null,
                fbfFieldID: 0,
                physicalPath: null,
              },
              fileID: 7694509,
              filePath: "",
            },
            {
              file: {
                id: 0,
                name: "logo.png",
                lastModified: 1721433600000,
                lastModifiedDate: "12/01/2024 13:10:56",
                webKitRelativePath: null,
                size: null,
                fbfFieldID: 0,
                physicalPath: null,
              },
              fileID: 7694510,
              filePath: "C:",
            },
            {
              file: {
                id: 0,
                name: "logo.png",
                lastModified: 1721433600000,
                lastModifiedDate: "12/01/2024 13:11:31",
                webKitRelativePath: null,
                size: null,
                fbfFieldID: 0,
                physicalPath: null,
              },
              fileID: 7694511,
              filePath: "",
            },
            {
              file: {
                id: 0,
                name: "logo.png",
                lastModified: 1721433600000,
                lastModifiedDate: "12/01/2024 13:12:04",
                webKitRelativePath: null,
                size: null,
                fbfFieldID: 0,
                physicalPath: null,
              },
              fileID: 7694512,
              filePath: "",
            },
          ],
          PK: "!!!SPNWCE-1733058728!!!",
          REPORTID: 81,
          FBFId: "5002",
          TaskCode: "312773",
          FormMode: "EDIT",
        };
        
        this.mapDropdownOptions(fields);
        this.type = "json-powered";
      });

    return;

  }

  mapDropdownOptions(fields: any) {
    // Simulating API call with fake data
    const fakeApiResponse: any = {

      sports: [
        { id: '1', name: 'Soccer' },
        { id: '2', name: 'Basketball' },
        { id: '3', name: 'sport 3' },
        { id: '4', name: 'sport 4' },

      ],
      teams: [
        { id: '1', name: 'Bayern Munich', parentId: '1' },
        { id: '2', name: 'Real Madrid', parentId: '1' },
        { id: '3', name: 'Cleveland', parentId: '2' },
        { id: '4', name: 'Miami', parentId: '2' },
      ],
      players: [
        { id: '1', name: 'Bayern Munich (Player 1)', parentId: '1' },
        { id: '2', name: 'Bayern Munich (Player 2)', parentId: '1' },
        { id: '3', name: 'Real Madrid (Player 1)', parentId: '2' },
        { id: '4', name: 'Real Madrid (Player 2)', parentId: '2' },
        { id: '5', name: 'Cleveland (Player 1)', parentId: '3' },
        { id: '6', name: 'Cleveland (Player 2)', parentId: '3' },
        { id: '7', name: 'Miami (Player 1)', parentId: '4' },
        { id: '8', name: 'Miami (Player 2)', parentId: '4' },
      ],
    };

    // Simulating API call using 'of' operator
    of(fakeApiResponse).subscribe(
      (data: any) => {
        // this.options.formState.selectOptionsData = data;

        this.options.formState = {
          ...this.options.formState,
          selectOptionsData: data,
        };

        //for non dependency lookups - not shared across props
        this.fields = this.mapFields(fields);
        this.showForm = true;
      },
      (error) => {
        console.error('Failed to fetch select options data:', error);
      }
    );
  }

  mapFields(fields: FormlyFieldConfig[]) {
    return fields.map((f: any) => {


      if (f.key === 'color') {
        f.type = 'radio';
        // f.props.options = this.getColors();
      }

      if (f.type === 'select' && f.props && f.props.multiple == true) {

        const valueAsNumArray = convertCommaSeparatedToNumberArray(this.model[f.key]);
        this.model[f.key] = valueAsNumArray;
      }

      return f;
    });
  }



  onSubmit() {
    console.log(this.model);
    if (this.form.valid) {
      // this.model = this.mapObjectProps(this.model);
      console.log(this.model);
    }

  }

  public ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
    this.subs.forEach((s) => s.unsubscribe());

  }

  modelChange(event: any) {
    //track model changes
    if (this.firstModelChange) {
      this.loadCityOptions();
      this.firstModelChange = false;
    }
    console.log(this.form);
    const allDropdownss = this.fields.filter(field => field.key === 'cityId');
    const allDropdown = this.fields.filter(field => field.type === 'select');

    allDropdown.forEach(select => {
      //we can call generic api to populate this select baseed on key
      //or to call all lookups data and make function to populate on the client
      if (select.key === 'formControl') {

      }

    });

    Object.keys(this.form.controls).forEach(key => {
      const controlErrors = this.form?.get(key)?.errors;
      if (controlErrors != null) {

      }
    });


  }

  loadCityOptions() {
    const cityFields = this.fields.find(field => field.key === 'nationId');

    if (cityFields) {
      cityFields.formControl?.valueChanges.subscribe(value => {
        console.log('nationId changes:', value);
      });
    }

  }


}
