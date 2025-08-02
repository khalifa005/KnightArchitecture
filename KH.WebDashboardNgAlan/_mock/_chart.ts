import { format } from 'date-fns';
import * as Mock from 'mockjs';

// region: mock data

const visitData: any[] = [];
const beginDay = new Date().getTime();

const fakeY = [7, 5, 4, 2, 4, 7, 5, 6, 5, 9, 6, 3, 1, 5, 3, 6, 5];
for (let i = 0; i < fakeY.length; i += 1) {
  visitData.push({
    x: format(new Date(beginDay + 1000 * 60 * 60 * 24 * i), 'yyyy-MM-dd'),
    y: fakeY[i]
  });
}

const visitData2: any[] = [];
const fakeY2 = [1, 6, 4, 8, 3, 7, 2];
for (let i = 0; i < fakeY2.length; i += 1) {
  visitData2.push({
    x: format(new Date(beginDay + 1000 * 60 * 60 * 24 * i), 'yyyy-MM-dd'),
    y: fakeY2[i]
  });
}

const salesData: any[] = [];
for (let i = 0; i < 12; i += 1) {
  salesData.push({
    x: `Month ${i + 1}`,
    y: Math.floor(Math.random() * 1000) + 200
  });
}
const searchData: any[] = [];
for (let i = 0; i < 50; i += 1) {
  searchData.push({
    index: i + 1,
    keyword: `Search Keyword-${i}`,
    count: Math.floor(Math.random() * 1000),
    range: Math.floor(Math.random() * 100),
    status: Math.floor((Math.random() * 10) % 2)
  });
}
const salesTypeData = [
  {
    x: 'Home Appliances',
    y: 4544
  },
  {
    x: 'Food & Beverages',
    y: 3321
  },
  {
    x: 'Personal Care & Health',
    y: 3113
  },
  {
    x: 'Clothing & Bags',
    y: 2341
  },
  {
    x: 'Baby & Mother Products',
    y: 1231
  },
  {
    x: 'Others',
    y: 1231
  }
];

const salesTypeDataOnline = [
  {
    x: 'Home Appliances',
    y: 244
  },
  {
    x: 'Food & Beverages',
    y: 321
  },
  {
    x: 'Personal Care & Health',
    y: 311
  },
  {
    x: 'Clothing & Bags',
    y: 41
  },
  {
    x: 'Baby & Mother Products',
    y: 121
  },
  {
    x: 'Others',
    y: 111
  }
];

const salesTypeDataOffline = [
  {
    x: 'Home Appliances',
    y: 99
  },
  {
    x: 'Personal Care & Health',
    y: 188
  },
  {
    x: 'Clothing & Bags',
    y: 344
  },
  {
    x: 'Baby & Mother Products',
    y: 255
  },
  {
    x: 'Others',
    y: 65
  }
];

const offlineData: any[] = [];
for (let i = 0; i < 10; i += 1) {
  offlineData.push({
    name: `Store ${i}`,
    cvr: Math.ceil(Math.random() * 9) / 10
  });
}
const offlineChartData: any[] = [];
for (let i = 0; i < 20; i += 1) {
  offlineChartData.push({
    time: new Date().getTime() + 1000 * 60 * 30 * i,
    y1: Math.floor(Math.random() * 100) + 10,
    y2: Math.floor(Math.random() * 100) + 10
  });
}

const radarOriginData = [
  {
    name: 'Individual',
    ref: 10,
    koubei: 8,
    output: 4,
    contribute: 5,
    hot: 7
  },
  {
    name: 'Team',
    ref: 3,
    koubei: 9,
    output: 6,
    contribute: 3,
    hot: 1
  },
  {
    name: 'Department',
    ref: 4,
    koubei: 1,
    output: 6,
    contribute: 5,
    hot: 7
  }
];

//
const radarData: any[] = [];
const radarTitleMap: any = {
  ref: 'Reference',
  koubei: 'Reputation',
  output: 'Output',
  contribute: 'Contribution',
  hot: 'Hot'
};
radarOriginData.forEach((item: any) => {
  Object.keys(item).forEach(key => {
    if (key !== 'name') {
      radarData.push({
        name: item.name,
        label: radarTitleMap[key],
        value: item[key]
      });
    }
  });
});

// endregion

export const CHARTS = {
  '/chart': JSON.parse(
    JSON.stringify({
      visitData,
      visitData2,
      salesData,
      searchData,
      offlineData,
      offlineChartData,
      salesTypeData,
      salesTypeDataOnline,
      salesTypeDataOffline,
      radarData
    })
  ),
  '/chart/visit': JSON.parse(JSON.stringify(visitData)),
  '/chart/tags': Mock.mock({
    'list|100': [{ name: '@city', 'value|1-100': 150 }]
  })
};
