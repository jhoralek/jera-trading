<template>
    <div class="registration">
        <v-layout row>
            <v-flex xs12 justify-end align-center>
                <h3 v-if="state < 4">{{ state }}/3</h3>
            </v-flex>
        </v-layout>
        <v-form lazy-validation ref="step1" v-if="profile.user && state === 1">
            <v-layout row wrap>
                <v-flex xs12>
                    <v-text-field v-model="newUser.userName"
                                  v-validate="'required|regex:^\\S*$|max:25|min:4|userNameUnique'"
                                  :error-messages="errors.collect('userName')"
                                  data-vv-name="userName"
                                  couter
                                  :label="labelUserName" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12>
                    <v-text-field :label="labelPassword"
                                  v-model="newUser.password"
                                  :append-icon="visible ? 'visibility_off' : 'visibility'"
                                  :append-icon-cb="() => (visible = !visible)"
                                  :type="visible ? 'text' : 'password'"
                                  counter
                                  v-validate="'required|max:25|min:4'"
                                  :error-messages="errors.collect('password')"
                                  data-vv-name="password" />
                    <v-text-field :label="labelRepeatPassword"
                                  v-model="repPassword"
                                  :append-icon="visibleRep ? 'visibility_off' : 'visibility'"
                                  :append-icon-cb="() => (visibleRep = !visibleRep)"
                                  :type="visibleRep ? 'text' : 'password'"
                                  counter
                                  v-validate="{is:newUser.password, required: true}"
                                  :error-messages="errors.collect('repPassword')"
                                  data-vv-name="repPassword" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12 md8>
                    <v-checkbox v-validate="'required'"
                                v-model="newUser.isAgreementToTerms"
                                :error-messages="errors.collect('isAgreementToTerms')"
                                :label="labelIsAgreementToTerms"
                                data-vv-name="isAgreementToTerms"
                                :value="true"
                                type="checkbox" />
                </v-flex>
                <v-flex xs12 md4>
                    <v-checkbox v-model="newUser.sendingNews"
                                :label="labelSendingNews"
                                :value="false"
                                type="checkbox" />
                </v-flex>
            </v-layout>
        </v-form>
        <v-form lazy-validation ref="step2" v-if="profile.user && state === 2">
            <v-layout row wrap>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.firstName"
                                  v-validate="'required'"
                                  :error-messages="errors.collect('firstName')"
                                  data-vv-name="firstName"
                                  couter
                                  :label="labelFirstName" />
                </v-flex>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.lastName"
                                  v-validate="'required'"
                                  :error-messages="errors.collect('lastName')"
                                  data-vv-name="lastName"
                                  couter
                                  :label="labelLastName" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.titleBefore"
                                  :label="labelTitleBefore" />
                </v-flex>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.titleAfter"
                                  :label="labelTitleAfter" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.email"
                                  v-validate="'email|required|emailUnique'"
                                  :error-messages="errors.collect('email')"
                                  data-vv-name="email"
                                  couter
                                  :label="labelEmail" />
                </v-flex>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.phoneNumber"
                                  v-validate="{ required: true, regex: /^([0-9 ]+)$/, max: 15, min: 9}"
                                  :error-messages="errors.collect('phoneNumber')"
                                  data-vv-name="phoneNumber"
                                  couter
                                  :label="labelPhoneNumber" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.birthNumber"
                                  v-validate="{ min:9, max:15, regex: /^([0-9\/]+)$/ }"
                                  :error-messages="errors.collect('birthNumber')"
                                  data-vv-name="birthNumber"
                                  couter
                                  :label="labelBirthNumber" />
                </v-flex>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.webPageUrl"
                                  v-validate="'url'"
                                  :error-messages="errors.collect('webPageUrl')"
                                  data-vv-name="webPageUrl"
                                  couter
                                  :label="labelWebPageUrl" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12>
                    <v-text-field v-model="newCustomer.companyName"
                                  v-validate="'max:100'"
                                  :error-messages="errors.collect('companyName')"
                                  data-vv-name="companyName"
                                  couter
                                  :label="labelCompanyName" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.companyNumber"
                                  v-validate="'max:20'"
                                  :error-messages="errors.collect('companyNumber')"
                                  data-vv-name="companyNumber"
                                  couter
                                  :label="labelCompanyNumber" />
                </v-flex>
                <v-flex xs12 md6>
                    <v-text-field v-model="newCustomer.companyLegalNumber"
                                  v-validate="'max:20'"
                                  :error-messages="errors.collect('companyLegalNumber')"
                                  data-vv-name="companyLegalNumber"
                                  couter
                                  :label="labelCompanyLegalNumber" />
                </v-flex>
            </v-layout>
        </v-form>
        <v-form lazy-validation ref="step3" v-if="profile.user && state === 3">
            <v-layout row wrap>
                <v-flex xs12>
                    <v-text-field v-model="newAddress.street"
                                  v-validate="'required|max:30'"
                                  :error-messages="errors.collect('stret')"
                                  data-vv-name="stret"
                                  :label="labelStreet" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12 md8>
                    <v-text-field v-model="newAddress.city"
                                  v-validate="'required|max:30'"
                                  :error-messages="errors.collect('city')"
                                  data-vv-name="city"
                                  :label="labelCity" />
                </v-flex>
                <v-flex xs12 md4>
                    <v-text-field v-model="newAddress.postCode"
                                  v-validate="'required|max:10'"
                                  :error-messages="errors.collect('postCode')"
                                  data-vv-name="postCode"
                                  :label="labelPostCode" />
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12>
                    <v-select v-model="newAddress.countryId"
                              v-validate="'required'"
                              data-vv-name="countryId"
                              :error-messages="errors.collect('countryId')"
                              item-value="id"
                              item-text="name"
                              single-line
                              :label="labelCountry"
                              :items="countries" />
                </v-flex>
            </v-layout>
        </v-form>
        <v-container v-if="profile.user && state === 4">
            <v-layout row wrap>
                <v-flex xs12 class="text-xs-center">
                    <h3>{{ resx('registrationSent') }}</h3>
                </v-flex>
                <v-flex xs12 class="text-xs-center">
                    <p>{{ resx('registrationCompleteInfo') }}</p>
                </v-flex>
                <v-flex xs12 class="text-xs-center">
                    <v-icon size="100px" color="green darken-2">check_circle_outline</v-icon>
                </v-flex>
            </v-layout>
        </v-container>
        <v-btn v-if="state > 1 && state < 4"
               color="black"
               @click="previous">
            <v-icon left dark>keyboard_arrow_left</v-icon>
            {{ resx('back') }}
        </v-btn>
        <v-btn v-if="state !== 3 && state < 4"
               color="black"
               @click="next">
            {{ resx('next') }}
            <v-icon left dark>keyboard_arrow_right</v-icon>
        </v-btn>
        <v-btn v-if="state === 3"
               color="black"
               @click="submit">
            {{ resx('submit') }}
        </v-btn>
    </div>
</template>

<script lang="ts">

    import { Component, Vue } from 'vue-property-decorator';
    import { State, Action, namespace, Getter } from 'vuex-class';

    import FormBaseComponent from '../FormBaseComponent.vue';
    import { ProfileState, SettingsState } from './../../store/types';
    import {
        User,
        Country,
        Customer,
        Address,
    } from './../../model';
    import { ExtendOptions, Rule } from 'vee-validate';
    import { log } from 'util';

    const ProfileAction = namespace('profile', Action);
    const SettingsGetter = namespace('settings', Getter);

    @Component({})
    export default class RegistrationComponent extends FormBaseComponent {
        @State('profile')
        private profile: ProfileState;

        @ProfileAction('setCurrentUser')
        private setUser: any;
        @ProfileAction('setCurrentUsersCustomer')
        private setCustomer: any;
        @ProfileAction('setCurrentUserCustomersAddress')
        private setAddress: any;
        @ProfileAction('newUser')
        private saveUser: any;
        @ProfileAction('checkUserName')
        private validateUser: any;
        @ProfileAction('checkEmail')
        private validateEmail: any;

        @SettingsGetter('getCountries')
        private countries: Country[];

        private newUser: User = {
            isAgreementToTerms: null,
            isActive: false,
            sendingNews: false,
            created: new Date(),
        } as User;
        private newCustomer: Customer = {
            isDealer: false,
            isFeePayed: false,
            created: new Date(),
        } as Customer;
        private newAddress: Address = {
            street: undefined,
            created: new Date(),
        } as Address;

        private state: number = 1;
        private repPassword: string = '';
        private visiblePwd: boolean = false;
        private visibleRepPwd: boolean = false;

        public mounted() {
            const userNameRule = {
                getMessage: (field) => `${field} ${this.isAlreadyUseMessage}`,
                validate: (value) => new Promise((resolve) => {
                    return this.validateUser(value).then((response) => {
                        return resolve({
                            valid: response as boolean,
                        });
                    });
                }),
            } as Rule;

            const emailRule = {
                getMessage: (field) => `${field} ${this.isAlreadyUseMessage}`,
                validate: (value) => new Promise((resolve) => {
                    return this.validateEmail(value).then((response) => {
                        return resolve({
                            valid: response as boolean,
                        });
                    });
                }),
            } as Rule;

            this.$validator.extend('userNameUnique',
                userNameRule,
                { imediate: false } as ExtendOptions);
            this.$validator.extend('emailUnique',
                emailRule,
                { imediate: false } as ExtendOptions);
        }

        private labelName(label: string): string {
            return this.settings.resource[label];
        }

        private previous(): void {
            if (this.state > 1) {
                this.state -= 1;
            }
        }

        private next(): void {
            this.$validator.validateAll().then((response) => {
                if (response) {
                    switch (this.state) {
                        case 1:
                            this.newUser.customer = this.newCustomer;
                            this.setUser(this.newUser).then((nUserResponse) => {
                                if (nUserResponse) {
                                    this.state += 1;
                                }
                            });
                            break;
                        case 2:
                            this.newUser.customer.address = this.newAddress;
                            this.setCustomer(this.newCustomer).then((nCustResponse) => {
                                if (nCustResponse) {
                                    this.state += 1;
                                }
                            });
                            break;
                    }
                }
            });
        }

        private submit(): void {
            this.$validator.validateAll().then((response) => {
                if (response) {
                    if (this.state === 3) {
                        this.newAddress.country = this.countries
                            .find((f) => f.id === this.newAddress.countryId);

                        this.setAddress(this.newAddress).then((nAddrResponse) => {
                            if (nAddrResponse) {
                                this.saveUser(this.profile.user).then((saveResponse) => {
                                    this.state += 1;
                                });
                            }
                        });
                    }
                }
            });
        }

        get isAlreadyUseMessage(): string {
            return this.settings.resource.isAlreadyUsed.toLowerCase();
        }

        get labelUserName(): string {
            return this.settings.resource.userName;
        }

        get labelStreet(): string {
            return this.settings.resource.street;
        }

        get labelCity(): string {
            return this.settings.resource.city;
        }

        get labelPostCode(): string {
            return this.settings.resource.postCode;
        }

        get labelCountry(): string {
            return this.settings.resource.country;
        }

        get labelCompanyName(): string {
            return this.settings.resource.companyName;
        }

        get labelCompanyNumber(): string {
            return this.settings.resource.companyNumber;
        }

        get labelCompanyLegalNumber(): string {
            return this.settings.resource.companyLegalNumber;
        }

        get labelFirstName(): string {
            return this.settings.resource.firstName;
        }

        get labelLastName(): string {
            return this.settings.resource.lastName;
        }

        get labelEmail(): string {
            return this.settings.resource.email;
        }

        get labelPhoneNumber(): string {
            return this.settings.resource.phoneNumber;
        }

        get labelTitleAfter(): string {
            return this.settings.resource.titleAfter;
        }

        get labelTitleBefore(): string {
            return this.settings.resource.titleBefore;
        }

        get labelWebPageUrl(): string {
            return this.settings.resource.webPageUrl;
        }

        get labelBirthNumber(): string {
            return this.settings.resource.birthNumber;
        }

        get labelPassword(): string {
            return this.settings.resource.password;
        }

        get labelRepeatPassword(): string {
            return this.settings.resource.repeatPassword;
        }

        get labelSendingNews(): string {
            return this.settings.resource.sendingNews;
        }

        get labelIsAgreementToTerms(): string {
            return this.settings.resource.agreementToTerms;
        }

        get visible(): boolean {
            return this.visiblePwd;
        }
        set visible(value: boolean) {
            this.visiblePwd = value;
        }

        get visibleRep(): boolean {
            return this.visibleRepPwd;
        }

        set visibleRep(value: boolean) {
            this.visibleRepPwd = value;
        }
    }

</script>

<style>

    .registration .v-btn {
        color: white !important;
        border-radius: 5px !important;
    }
</style>