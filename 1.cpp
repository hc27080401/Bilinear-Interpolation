#include<cstdio>
#include<cstdlib>
#include<cstring>
#include<iostream>
using namespace std;
double a[100][100],b[100][100],c[100][100];
int da[16],db[16],dc[16],dd[16],bel[16][16];
int main()
{
    freopen("1.in","r",stdin);
    freopen("11.out","w",stdout);
    for (int i=0;i<16;i++)
        for (int j=0;j<16;j++)
            scanf("%lf",&a[i][j]);
    for (int i=0;i<16;i++)
        for (int j=0;j<16;j++)
            scanf("%lf",&b[i][j]);            
        for (int i=0;i<16;i++)
            for (int j=0;j<16;j++)            
                for (int k=0;k<16;k++)
                {
                    double tmp=a[i][k]*b[k][j];
                    c[i][j]=c[i][j]+tmp;
                }
    printf("%.2lf %.2lf\n",a[0][0],b[0][5]);    

    printf("\n");
    for (int i=0;i<16;i++)
    {
        for (int j=0;j<16;j++)
            printf("%.2lf ",b[i][j]);
        printf("\n");
    }    
    printf("above was b:\n");
    for (int i=0;i<16;i++)
    {
        for (int j=0;j<16;j++)
            printf("%.2lf ",c[i][j]);
        printf("\n");
    }    
    int cnt=0;        
    for (int i=0;i<4;i++)
    {
        for (int j=0;j<4;j++)
        {
            da[cnt]=j;
            db[cnt]=i;   
            bel[j][i]=cnt;    
            cnt++;
        }
    }
    cnt=0;
    for (int i=0;i<4;i++)
    {
        for (int j=0;j<4;j++)
        {
            dc[cnt]=i;
            dd[cnt]=j;       
            cnt++;
        }
    }
    for (int i=0;i<16;i++) printf("%d %d\n",da[i],db[i]);    
    for (int i=0;i<16;i++)
    {
        printf("a[%d,%d] = ",dc[i],dd[i]);
        for (int j=0;j<16;j++)
        {
            if (c[bel[dc[i]][dd[i]]][j])
            {
                printf("%.2lf*p[%d,%d]+",c[bel[dc[i]][dd[i]]][j],dc[j],dd[j]);
            }
        }
        printf(";\n");
    }
    printf("%.2lf %.2lf %.2lf\n",c[5][0],a[5][12],b[12][0]);
    return 0;
}
